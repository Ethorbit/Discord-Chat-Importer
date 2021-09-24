using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ImportEventArgs
	{
		ISocketMessageChannel Channel { get; }

		public ImportEventArgs(ISocketMessageChannel channel)
		{
			this.Channel = channel;
		}
	}
}
