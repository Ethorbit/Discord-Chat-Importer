using System;
using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.Utilities;
using Discord_Channel_Importer.DiscordBot.Export;
using Discord_Channel_Importer.DiscordBot.Settings;
using Discord.WebSocket;
using Discord;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
	internal class Bot : IBot
	{
		public event EventHandler<EventArgs> Logged_In;
		public BotSettings Settings { get; }
		public ChatImportManager ChatImportManager { get; }

		public Bot(BotSettings settings, ChatImportManager importManager)
		{
			this.Settings = settings;
			this.ChatImportManager = importManager;
			this.ChatImportManager.StartImportLoop();
			this.ChatImportManager.ImportFinished += ChatImportManager_ImportFinished;
		}

		public async Task StartAsync()
		{	
			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}

		/// <inheritdoc cref="IBot.ImportMessagesFromURIToChannelAsync"/>
		/// <returns>
		/// BotReturn.ImporterExists, BotReturn.ParseError, BotReturn.Success
		/// </returns>
		/// <param name="importReady">Callback function to retrieve the created ChatImporter</param>
		public async Task<BotReturn> ImportMessagesFromURIAsync(Uri uri, IUser caller, ISocketMessageChannel channel, Action<IChatImporter> callback = null)
		{
			if (this.ChatImportManager.ChannelHasImporter(channel))
				return BotReturn.ImporterExists;

			try
			{
				object exportedObj = await Web.GetJsonFromURIAsync(uri, typeof(ExportedChannel));
				var exportedChannel = (ExportedChannel)exportedObj;

				var importer = new ChatImporter(new ChatImporterSettings(caller, channel, exportedChannel));

				if (callback != null)
					callback(importer);

				return BotReturn.Success;
			}
			catch 
			{
				return BotReturn.ParseError;
			}
		}

		/// <inheritdoc cref="IBot.CancelImportingToChannelAsync"/>
		/// <param name="uri"></param>
		/// <param name="toChannel"></param>
		/// <returns>
		/// BotReturn.ImporterDoesntExist, BotReturn.Success
		/// </returns>
		public async Task<BotReturn> CancelImportingToChannelAsync(ISocketMessageChannel channel)
		{
			if (!this.ChatImportManager.ChannelHasImporter(channel))
				return BotReturn.ImporterDoesntExist;

			this.ChatImportManager.RemoveImporter(channel);
			return BotReturn.Success;
		}

		/// <inheritdoc cref="IBot.RemoveArchivedMessagesFromChannelAsync"/>
		/// <param name="uri"></param>
		/// <param name="toChannel"></param>
		/// <returns>
		/// BotReturn.Success
		/// </returns>
		public async Task<BotReturn> RemoveArchivedMessagesFromChannelAsync(ISocketMessageChannel channel)
		{
			await this.CancelImportingToChannelAsync(channel);

			return BotReturn.Success;
		}

		/// <summary>
		/// When we finish importing to a channel
		/// </summary>
		private void ChatImportManager_ImportFinished(object sender, ChatImportManagerEventArgs e)
		{
	
		}
	}
}
