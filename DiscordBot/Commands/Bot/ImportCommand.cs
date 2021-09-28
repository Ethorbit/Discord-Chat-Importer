using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.Extensions;
using Discord_Channel_Importer.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class ImportCommand : CommandModule
	{
		public override CommandInfo[] CommandInfo { get; } = new CommandInfo[]
		{ 
			new CommandInfo() 
			{  
				Usage = "import <url> #channel-name",
				Description = "Gets all Discord messages from the provided URL (which should be a .json with the proper Discord channel structure) and recreates them in the provided channel."
			}
		};

		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer import", true, RunMode = RunMode.Async)]
		public async Task ImportMessages(string url, string channel = "")
		{
			await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.InvalidChannel));
		}

		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer import", true, RunMode = RunMode.Async)]
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

			if (this.Context.Bot.ChatImportManager.ChannelHasImporter(channel))
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Duplicate import!", "I am already importing to that channel, please wait until I finish..", Color.Red));
				return;
			}		
			#endregion

			IUserMessage WaitMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Please wait...", "I am downloading the text from that URL, this could take some time.", Color.LightGrey));

			try
			{
				object exportedObj = await Json.GetJsonObjectFromURIAsync(uri, typeof(ExportedChannel));
				await WaitMsg.DeleteAsync();

				var exportedChannel = (ExportedChannel)exportedObj;
				await this.ConfirmImportAsync(new ChatImporter(new ChatImporterSettings(this.Context.User, channel, exportedChannel)));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				await WaitMsg.DeleteAsync();

				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Error parsing the URL!", "There was an error parsing the text on the provided webpage. Reasons why this might happen:", Color.Red,
					new EmbedField[] {
					DiscordFactory.CreateEmbedField("It's not raw text", "Use Inspect Element on the page, if there's any scripting on it at all: it is not Raw Text.", true),
					DiscordFactory.CreateEmbedField("Incompatible .json", "The .json's structure does not match the requirements, you must export the channel's .json with DiscordChatExporter because I do not support anything else.", true),
					DiscordFactory.CreateEmbedField("Connection error", "There may have been a connection issue out of your control, try again later..", true)
					}
				));
			}
		}

		private async Task ConfirmImportAsync(ChatImporter importer)
		{
			importer.Settings.IsEnabled = false;

			ChatImportManager chatImportManager = this.Context.Bot.ChatImportManager;
			chatImportManager.AddImporter(importer);

			TimeSpan estimatedTime = chatImportManager.GetEstimatedImportTime(importer.Settings.Destination);

			// Confirmation message
			IUserMessage reactMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed
			(
				"Import Confirmation",
				$@"Are you sure you want to do this? With {chatImportManager.Importers.Count} concurrent imports and {importer.Settings.Source.Messages.Count} messages, 
				this will take an estimated **{estimatedTime.ToReadableFormat()}** to complete. 
				
				You may want to go back and hide the channel first so that users aren't spammed.",
				Color.Orange)
			);

			await this.Context.Bot.WaitForMessageReactionsAsync(this.Context.Guild, reactMsg, new List<string>() { "✅", "❌" }, DefaultPermission, true, async (SocketReaction reaction) =>
			{
				if (reaction.Emote.Name == "✅") // Proceed with import
				{
					importer.Settings.IsEnabled = true;
					await reactMsg.DeleteAsync();	
				}
				else if (reaction.Emote.Name == "❌") // Cancel
				{
					chatImportManager.RemoveImporter(importer.Settings.Destination);
					await reactMsg.DeleteAsync();
				}
			});

			importer.FinishImports += Importer_FinishImports;
		}

		private async void Importer_FinishImports(object sender, IChatImporterSettings e)
		{
			await e.Destination.SendMessageAsync(e.Requester.Mention + " importing for this channel has finished. :thumbsup:");
		}
	}
}
