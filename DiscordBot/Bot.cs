using System;
using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.Utilities;
using Discord_Channel_Importer.DiscordBot.Export;
using Discord_Channel_Importer.DiscordBot.Settings;
using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
	public class Bot : IBot
	{
		public event EventHandler<EventArgs> Logged_In;
		public BotSettings Settings { get; }
		public ChatImportManager ChatImportManager { get; }

		public Bot(BotSettings settings, ChatImportManager importManager)
		{
			this.Settings = settings;
			this.ChatImportManager = importManager;
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
		/// <param name="uri"></param>
		/// <param name="toChannel"></param>
		/// <returns>
		/// BotReturn.ImporterExists, BotReturn.MaxImportsReached, BotReturn.ParseError, BotReturn.Success
		/// </returns>
		public async Task<BotReturn> ImportMessagesFromURIToChannelAsync(Uri uri, ISocketMessageChannel channel)
		{
			if (this.ChatImportManager.ChannelHasImporter(channel))
				return BotReturn.ImporterExists;

			if (this.ChatImportManager.HasMaxImporters)
				return BotReturn.MaxImportsReached;

			try
			{
				object exportedObj = await Web.GetJsonFromURIAsync(uri, typeof(ExportedChannel));
				var exportedChannel = (ExportedChannel)exportedObj;

				ChatImporter importer = this.ChatImportManager.AddImporter(channel, exportedChannel);
				// TODO: start the import process.

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
	}
}
