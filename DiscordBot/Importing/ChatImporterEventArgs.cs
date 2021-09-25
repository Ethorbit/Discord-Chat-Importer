using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ChatImporterEventArgs
	{
		public ISocketMessageChannel Channel { get; }

		public ChatImporterEventArgs(ISocketMessageChannel channel)
		{
			this.Channel = channel;
		}
	}
}
