using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.ImportStructures;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// Settings for a ChatImporter
	/// </summary>
	public class ChatImporterSettings : IChatImporterSettings
	{
		public Timer ImportTimer { get; }
		public ISocketMessageChannel ImportChannel { get; }
		public ExportedChannel ExportedChannel { get; }

		public ChatImporterSettings(ISocketMessageChannel importChannel, ExportedChannel exportedChannel)
		{
			this.ImportChannel = importChannel;
			this.ExportedChannel = exportedChannel;
			this.ImportTimer = new Timer(1000);
		}
		public ChatImporterSettings(ISocketMessageChannel importChannel, ExportedChannel exportedChannel, Timer importTimer)
		{
			this.ImportChannel = importChannel;
			this.ExportedChannel = exportedChannel;
			this.ImportTimer = importTimer;
		}
	}
}
