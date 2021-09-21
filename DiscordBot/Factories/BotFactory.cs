
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Commands;

namespace Discord_Channel_Importer.DiscordBot.Factories
{
	public static class BotFactory
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

			var bot = new DiscordBot.Bot(new DiscordBot.Settings.BotSettings(new DiscordSocketClient(socketConfig), botToken), new DiscordBot.Importing.ChatImporter());

			var commandHandler = new CommandHandler(bot, new CommandService());
			await commandHandler.StartAsync();

			return bot;
		}
	}
}
