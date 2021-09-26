using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ChatImporterSettings : IChatImporterSettings
	{
		public IUser Requester { get; }
		public ISocketMessageChannel Destination { get; }
		public ExportedChannel Source { get; }

		public ChatImporterSettings(IUser requester, ISocketMessageChannel destination, ExportedChannel source)
		{
			this.Requester = requester;
			this.Destination = destination;
			this.Source = source;
		}
	}
}
