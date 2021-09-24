using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Importing;

namespace Discord_Channel_Importer.DiscordBot
{
	internal interface IBotSettings
	{
		public DiscordSocketClient Client { get; }
		public string Token { get; }
	}
}
