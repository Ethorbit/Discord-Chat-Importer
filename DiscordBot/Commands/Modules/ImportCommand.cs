using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Importing;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class ImportCommand : CommandModule
	{
		public Emoji CheckEmoji { get; } = new Emoji("✅");
		public Emoji XEmoji { get; } = new Emoji("❌");

		[RequireUserPermissionWithError(CommandPermissions, Group = "Permission")]
		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url, string channel = "")
		{
			if (channel.Length <= 0)
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Channel required!", "You want to import messages into a channel, but what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
			else
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Invalid channel!", "What is that? You need to tag the channel (#my-channel)", Color.Red));
		}

		[RequireUserPermissionWithError(CommandPermissions, Group = "Permission")]
		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", ISocketMessageChannel channel = null)
		{
			#region Attempt
			if (url.Length <= 0)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("URL required!", "How am I supposed to know what you want? Provide a URL to the json containing the channel with messages.", Color.Red));
				return;
			}

			Uri uri;
			Uri.TryCreate(url, UriKind.Absolute, out uri);

			if (uri == null)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Invalid URL!", "The URL you passed is not valid. (It should look something like this: https://my-website.com/)", Color.Red));
				return;
			}
			#endregion

			IUserMessage WaitMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Please wait...", "I am downloading the text from that URL, this could take some time.", Color.LightGrey));

			var importCallback = new Action<IChatImporter>(async (IChatImporter importer) =>
			{
				IUserMessage reactMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Import Confirmation",
								$@"Are you sure you want to do this? With {Context.Bot.ChatImportManager.Importers.Count} 
								concurrent imports and {importer.Settings.Source.Messages.Count} messages, this will take an estimated {this.Context.Bot.ChatImportManager.GetEstimatedImportTime()} to complete.
																		
								You may want to go back and hide the channel first so that users aren't spammed.",
								Color.Orange));

				try
				{
					await reactMsg.AddReactionsAsync(new IEmote[] { this.CheckEmoji, this.XEmoji });

					this.Context.Client.ReactionAdded += async (Cacheable<IUserMessage, ulong> reactedMsg, ISocketMessageChannel arg2, SocketReaction arg3) =>
					{
						if (reactedMsg.Id == reactMsg.Id)
						{
							if (!arg3.User.IsSpecified)
								return;

							var user = arg3.User.Value;
							user = this.Context.Guild.GetUser(user.Id);

							if (user is IGuildUser userGuild && userGuild.GuildPermissions.Has(CommandPermissions))
							{
								if (arg3.Emote.Name == CheckEmoji.Name)
								{
									this.Context.Bot.ChatImportManager.AddImporter(channel, importer.Settings.Source);
								}
								else if (arg3.Emote.Name == XEmoji.Name)
								{
									await reactMsg.DeleteAsync();
								}
							}
						}		
					};
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}

				//importer.StartImport();
				importer.FinishImports += async (object e, ChatImporterEventArgs args) =>
				{
					await ReplyAsync(Context.User.Mention + " importing for channel: " + channel.Name + $" has finished.");
				};
			});

			try
			{
				BotReturn res = await this.Context.Bot.ImportMessagesFromURIAsync(uri, channel, importCallback);

				//if (res == BotReturn.MaxImportsReached)
				//	await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Too many imports!", "I'm too busy handling the other imports right now, try again later.", Color.Red));

				if (res == BotReturn.ImporterExists)
					await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Duplicate import!", "I am already importing to that channel, please wait until I finish..", Color.Red));

				await WaitMsg.DeleteAsync();

				if (res == BotReturn.ParseError)
				{
					await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Error parsing the URL!", "There was an error parsing the text on the provided webpage. Reasons why this might happen:", Color.Red,
						new EmbedField[] {
							DiscordFactory.CreateEmbedField("It's not raw text", "Use Inspect Element on the page, if there's any scripting on it at all: it is not Raw Text.", true),
							DiscordFactory.CreateEmbedField("Incompatible .json", "The .json's structure does not match the requirements, you must export the channel's .json with DiscordChatExporter because I do not support anything else.", true),
							DiscordFactory.CreateEmbedField("Connection error", "There may have been a connection issue out of your control, try again later..", true)
						}
					));

					return;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}


	}
}
