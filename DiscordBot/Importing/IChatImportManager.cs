using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	public interface IChatImportManager
	{
		public Dictionary<IChannel, ChatImporter> Importers { get; }

		public int MaxSimultaneousImports { get; }

		public bool HasMaxImporters { get; }


		public bool ChannelHasImporter(IChannel channel);

		public ChatImporter AddImporter(IChannel channel);

		public ChatImporter GetImporter(IChannel channel);

		public void RemoveImporter(IChannel channel);
	}
}
