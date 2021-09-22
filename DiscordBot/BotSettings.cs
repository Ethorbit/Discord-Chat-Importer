using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Importing;

namespace Discord_Channel_Importer.DiscordBot.Settings
{
	public class BotSettings : IBotSettings
	{
		public DiscordSocketClient Client { get; }
		public string Token { get; }

		public BotSettings(DiscordSocketClient client, string token)
		{
			this.Client = client;
			this.Token = token;
		}
	}
}
