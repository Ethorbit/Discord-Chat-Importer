using System;
using System.Threading.Tasks;
using Discord;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Commands;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.Utilities;
using Discord_Channel_Importer.DiscordBot.ImportStructures;
using Discord_Channel_Importer.DiscordBot.Settings;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
	public partial class Bot : IBot
	{
		public event EventHandler<EventArgs> Logged_In;

		public BotSettings Settings { get; }
		public IChatImporter ChatImporter { get; }

		public Bot(BotSettings settings, IChatImporter importer)
		{
			this.Settings = settings;
			this.ChatImporter = importer;
		}

		public async Task StartAsync()
		{
			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}

		public async Task LogCommandsAsync(BotSocketCommandContext cmdContext)
		{
			var channel = cmdContext.Channel;

			var embed = MessageFactory.CreateEmbed("Importer Commands", null, Color.LightGrey, new EmbedField[] {
				MessageFactory.CreateEmbedField("import <url> #channel-name", @"Gets all Discord messages from the 
																provided URL (which should be a .json 
																with the proper Discord channel structure) 
																and recreates them in the provided channel."),
				MessageFactory.CreateEmbedField("cancel #channel-name", @"Makes me stop importing to a channel that I am currently sending
																		archived messages to."),
				MessageFactory.CreateEmbedField("undo #channel-name", "Removes ALL archived messages I made in the specified channel and automatically cancels importing to that channel.")
			}); 

			await channel.SendMessageAsync(null, false, embed);
		}

		public async Task<ImportReturn> ImportMessagesFromURIToChannelAsync(Uri uri, IChannel toChannel)
		{
			try
			{
				object exportedObj = await Web.GetJsonFromURIAsync(uri, typeof(ExportedChannel));
				var exportedChannel = (ExportedChannel)exportedObj;

				// TODO: initialize ChatImporter and tell it to import all messages from exportedChannel

				return ImportReturn.SUCCESS;
			}
			catch 
			{
				return ImportReturn.PARSEERROR;
			}
		}

		public async Task CancelImportingToChannelAsync(BotSocketCommandContext cmdContext, IChannel toChannel)
		{
			if (cmdContext != null && toChannel == null)
			{
				await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Invalid Channel!", "What channel do you want to make me cancel importing to?", Color.Red));
			}

			// TODO: show message on success after successfully cancelling it
		}

		public async Task RemoveArchivedMessagesFromChannelAsync(BotSocketCommandContext cmdContext, IChannel channel)
		{
			if (channel == null)
			{
				await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Channel required!", "You want to remove all archived messages, but from what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			await this.CancelImportingToChannelAsync(null, channel);

			await Task.CompletedTask;
		}
	}
}
