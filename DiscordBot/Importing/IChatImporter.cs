using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// A Discord chat importer
	/// </summary>
	public interface IChatImporter
	{
		public IChatImporterSettings Settings { get; }
		public bool IsFinished { get; }

		public void StartImport();
		public void StopImport();
	}	
}
