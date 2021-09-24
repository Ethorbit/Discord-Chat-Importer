using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Reflection;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	internal class CommandHandler
	{
		private readonly DiscordBot.Bot _bot;
		private readonly DiscordSocketClient _client;
		private readonly CommandService _service;

		public CommandHandler(DiscordBot.Bot bot, CommandService commandService)
		{
			_bot = bot;
			_client = bot.Settings.Client;
			_service = commandService;
		}

		public async Task StartAsync()
		{
			_client.MessageReceived += Client_MessageReceived;
			await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
		}

		private async Task Client_MessageReceived(SocketMessage arg)
		{
			if (arg is SocketUserMessage) 
			{
				int argPos = 0;
				var userMsg = arg as SocketUserMessage;

				if (!userMsg.HasStringPrefix("!", ref argPos)) return;

				await _service.ExecuteAsync(new BotSocketCommandContext(_bot, userMsg), argPos, null);
			}
		}
	}
}
