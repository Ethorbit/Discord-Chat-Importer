using System;
using System.Threading.Tasks;
using Discord;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.Utilities;
using Discord_Channel_Importer.DiscordBot.ImportStructures;
using Discord_Channel_Importer.DiscordBot.Settings;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
	public class Bot : IBot
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

		public async Task<BotReturn> ImportMessagesFromURIToChannelAsync(Uri uri, IChannel toChannel)
		{
			try
			{
				object exportedObj = await Web.GetJsonFromURIAsync(uri, typeof(ExportedChannel));
				var exportedChannel = (ExportedChannel)exportedObj;

				// TODO: initialize ChatImporter and tell it to import all messages from exportedChannel

				return BotReturn.Success;
			}
			catch 
			{
				return BotReturn.Error;
			}
		}

		public async Task<BotReturn> CancelImportingToChannelAsync(IChannel toChannel)
		{
			// TODO: show message on success after successfully cancelling it
			return BotReturn.Success;
		}

		public async Task<BotReturn> RemoveArchivedMessagesFromChannelAsync(IChannel channel)
		{
			await this.CancelImportingToChannelAsync(channel);

			return BotReturn.Success;
		}
	}
}
