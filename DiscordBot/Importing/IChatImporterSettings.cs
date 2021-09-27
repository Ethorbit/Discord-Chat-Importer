using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal interface IChatImporterSettings
	{
		public IUser Requester { get; }
		public ISocketMessageChannel Destination { get; }
		public ExportedChannel Source { get; }
		public bool IsEnabled { get; set; }
	}
}
