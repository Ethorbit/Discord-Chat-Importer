using Discord;
using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Settings
{
	public class BotSettings
	{
		public readonly DiscordSocketClient Client;
		public readonly string Token;
		
		public BotSettings(DiscordSocketClient client, string token)
		{
			this.Client = client;
			this.Token = token;
		}
	}
}
