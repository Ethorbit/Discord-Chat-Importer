using Discord;
using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Settings
{
	public class BotSettings
	{
		public readonly DiscordSocketClient Client;
		public readonly string Token;
		public EmbedBuilder EmbedBuilder { get; set; }
		
		public BotSettings(DiscordSocketClient client, string token)
		{
			this.Client = client;
			this.Token = token;
		}
		public BotSettings(DiscordSocketClient client, string token, EmbedBuilder embedBuilder)
		{
			this.Client = client;
			this.Token = token;
			this.EmbedBuilder = embedBuilder;
		}
	}
}
