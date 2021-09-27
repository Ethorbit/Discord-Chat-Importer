using System;
using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.DiscordBot.Settings;
using Discord.WebSocket;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Commands;

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

		/// <summary>
		/// Creates our custom Discord bot.
		/// </summary>
		public static async Task<Bot> CreateBot(string botToken)
		{
			var socketConfig = new DiscordSocketConfig();
			var commandServiceConfig = new CommandServiceConfig();

#if DEBUG
			socketConfig.LogLevel = Discord.LogSeverity.Debug;
#else
			socketConfig.LogLevel = Discord.LogSeverity.Error;
#endif

			var bot = new Bot(new BotSettings(new DiscordSocketClient(socketConfig), botToken), new ChatImportManager());

			var commandHandler = new CommandHandler(bot, new CommandService());
			await commandHandler.StartAsync();

			return bot;
		}

		public Bot(BotSettings settings, ChatImportManager importManager)
		{
			this.Settings = settings;
			this.ChatImportManager = importManager;
			this.ChatImportManager.StartImportLoop();
		}

		public async Task StartAsync()
		{	
			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}
	}
}
