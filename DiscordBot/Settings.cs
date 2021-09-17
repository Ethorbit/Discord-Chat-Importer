using Discord.WebSocket;
using Discord.Commands;

namespace Discord_Channel_Importer.DiscordBot
{
	public class Settings
	{
		public DiscordSocketClient Client { get; }
		public CommandService CommandService { get; }
		public string Token { get; }

		public Settings(DiscordSocketClient client, string token)
		{
			this.Client = client;
			this.Token = token;
		}
		public Settings(DiscordSocketClient client, string token, CommandService commandService)
		{
			this.Client = client;
			this.Token = token;
			this.CommandService = commandService;
		}
	}
}
