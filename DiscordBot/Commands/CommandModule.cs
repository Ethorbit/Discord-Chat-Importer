using Discord;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Factories;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			try
			{
				await this.Context.Bot.LogCommandsAsync(this.Context);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", IChannel channel = null)
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

			await ReplyAsync(null, false, MessageFactory.CreateEmbed("Please wait...", "I am downloading the text from that URL, this could take some time.", Color.LightGrey));

			try
			{
				ImportReturn importRes = await this.Context.Bot.ImportMessagesFromURIToChannelAsync(uri, channel); //, this.Context);

				if (importRes == ImportReturn.SUCCESS)
				{
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Success!", "I finished the request.", Color.Green));
				}

				if (importRes == ImportReturn.PARSEERROR)
				{
					await ReplyAsync(null, false, MessageFactory.CreateEmbed("Error parsing the URL!", "There was an error parsing the text on the provided webpage. Reasons why this might happen:", Color.Red,
						new EmbedField[] {
							MessageFactory.CreateEmbedField("It's not raw text", "Use Inspect Element on the page, if there's any scripting on it at all; it is not Raw Text.", true),
							MessageFactory.CreateEmbedField("Incompatible .json", "The .json's structure does not match the requirements, you must export the channel's .json with DiscordChatExporter because I do not support anything else.", true),
							MessageFactory.CreateEmbedField("Connection error", "There may have been a connection issue out of your control, try again later..", true)
						}
					));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[Command("importer cancel", RunMode = RunMode.Async)]
		public async Task CancelMessageImport(IChannel channel)
		{
			try
			{
				await this.Context.Bot.CancelImportingToChannelAsync(this.Context, channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task UndoMessages(IChannel channel)
		{
			try 
			{
				await this.Context.Bot.RemoveArchivedMessagesFromChannelAsync(this.Context, channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}		
		}
	}
}
