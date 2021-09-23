
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.ImportStructures;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	public interface IChatImporterSettings
	{
		public Timer ImportTimer { get; }
		public ISocketMessageChannel ImportChannel { get; }
		public ExportedChannel ExportedChannel { get; }
	}
}
