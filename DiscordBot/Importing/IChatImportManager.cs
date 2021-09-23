using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.ImportStructures;
using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	public interface IChatImportManager
	{
		public Dictionary<IChannel, ChatImporter> Importers { get; }

		public int MaxSimultaneousImports { get; }

		public bool HasMaxImporters { get; }


		public bool ChannelHasImporter(ISocketMessageChannel channel);

		public ChatImporter AddImporter(ISocketMessageChannel channel, ExportedChannel export);

		public ChatImporter GetImporter(ISocketMessageChannel channel);

		public void RemoveImporter(ISocketMessageChannel channel);
	}
}
