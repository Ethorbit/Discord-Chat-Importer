
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

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

			var client = new DiscordSocketClient(socketConfig);

			var discordBotSettings = new DiscordBot.Settings.BotSettings(client, botToken);
			var bot = new DiscordBot.Bot(discordBotSettings);

			var commandHandler = new DiscordBot.CommandHandler(bot, new CommandService());
			await commandHandler.StartAsync();

			return bot;
		}
	}
}
