
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Commands;
using Discord_Channel_Importer.DiscordBot.Settings;
using Discord_Channel_Importer.DiscordBot.Importing;

namespace Discord_Channel_Importer.DiscordBot.Factories
{
	internal static class BotFactory
	{
		/// <summary>
		/// Creates our custom Discord bot.
		/// </summary>
		public static async Task<DiscordBot.Bot> CreateBot(string botToken)
		{
			var socketConfig = new DiscordSocketConfig();
			var commandServiceConfig = new CommandServiceConfig();

#if DEBUG
			socketConfig.LogLevel = Discord.LogSeverity.Debug;
#else
			socketConfig.LogLevel = Discord.LogSeverity.Error;
#endif

			var bot = new Bot(new BotSettings(new DiscordSocketClient(socketConfig), botToken), new ChatImportManager(new ChatImportManagerSettings(1, 1, 10)));

			var commandHandler = new CommandHandler(bot, new CommandService());
			await commandHandler.StartAsync();

			return bot;
		}
	}
}
