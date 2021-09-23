using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		private const GuildPermission _permissions = (GuildPermission.ManageMessages | GuildPermission.ManageChannels);


		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			await ReplyAsync(null, false, MessageFactory.CreateEmbed("Importer Commands", null, Color.LightGrey, 
				new EmbedField[] {
					MessageFactory.CreateEmbedField("import <url> #channel-name", @"Gets all Discord messages from the 
																	provided URL (which should be a .json 
																	with the proper Discord channel structure) 
																	and recreates them in the provided channel."),
					MessageFactory.CreateEmbedField("cancel #channel-name", @"Makes me stop importing to a channel that I am currently sending
																			archived messages to."),
					MessageFactory.CreateEmbedField("undo #channel-name", "Removes ALL archived messages I made in the specified channel and automatically cancels importing to that channel.")
				}
			));
		}

		[RequireUserPermissionWithError(_permissions, Group = "Permission")]
		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", ISocketMessageChannel channel = null)
		{
			#region Attempt
			if (url.Length <= 0)
			{
				await ReplyAsync(null, false, MessageFactory.CreateEmbed("URL required!", "How am I supposed to know what you want? Provide a URL to the json containing the channel with messages.", Color.Red));
				return;
			}

			if (channel == null)
			{
				await ReplyAsync(null, false, MessageFactory.CreateEmbed("Channel required!", "You want to import messages into a channel, but what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			Uri uri;
			Uri.TryCreate(url, UriKind.Absolute, out uri);

			if (uri == null)
			{
				await ReplyAsync(null, false, MessageFactory.CreateEmbed("Invalid URL!", "The URL you passed is not valid. (It should look something like this: https://my-website.com/)", Color.Red));
				return;
			}
			#endregion

			IUserMessage WaitMsg = await ReplyAsync(null, false, MessageFactory.CreateEmbed("Please wait...", "I am downloading the text from that URL, this could take some time.", Color.LightGrey));

			try
			{
				BotReturn res = await this.Context.Bot.ImportMessagesFromURIToChannelAsync(uri, channel); //, this.Context);

				if (res == BotReturn.MaxImportsReached)
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Too many imports!", "I'm way too busy handling the other imports right now, try again later."));

				if (res == BotReturn.ImporterExists)
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Duplicate import!", "I am already importing to that channel.."));
				
				if (res == BotReturn.Success)
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Success!", "I finished the request.", Color.Green));

				await WaitMsg.DeleteAsync();

				if (res == BotReturn.ParseError)
				{
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Error parsing the URL!", "There was an error parsing the text on the provided webpage. Reasons why this might happen:", Color.Red,
						new EmbedField[] {
							MessageFactory.CreateEmbedField("It's not raw text", "Use Inspect Element on the page, if there's any scripting on it at all: it is not Raw Text.", true),
							MessageFactory.CreateEmbedField("Incompatible .json", "The .json's structure does not match the requirements, you must export the channel's .json with DiscordChatExporter because I do not support anything else.", true),
							MessageFactory.CreateEmbedField("Connection error", "There may have been a connection issue out of your control, try again later..", true)
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

		[RequireUserPermissionWithError(_permissions, Group = "Permission")]
		[Command("importer cancel", RunMode = RunMode.Async)]
		public async Task CancelMessageImport(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, MessageFactory.CreateEmbed("Invalid Channel!", "What channel do you want to make me cancel importing to?", Color.Red));
				return;
			}

			try
			{
				BotReturn res = await this.Context.Bot.CancelImportingToChannelAsync(channel);

				if (res == BotReturn.ImporterDoesntExist)
				{
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Not importing!", "I am not importing messages to that channel.", Color.Red));
					return;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[RequireUserPermissionWithError(_permissions, Group = "Permission")]
		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task RemoveImportedMessages(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, MessageFactory.CreateEmbed("Channel required!", "You want to remove all archived messages, but from what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			try 
			{
				BotReturn res = await this.Context.Bot.RemoveArchivedMessagesFromChannelAsync(channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}		
		}
	}
}
