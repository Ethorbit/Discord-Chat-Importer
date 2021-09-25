using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// For Discord channel chat importers
	/// </summary>
	internal interface IChatImporter
	{
		public event EventHandler<ChatImporterEventArgs> FinishImports;
		public ISocketMessageChannel Destination { get; }
		public ExportedChannel Source { get; }
		bool IsFinished { get; }


		Task ImportNextMessage();
	}	
}
