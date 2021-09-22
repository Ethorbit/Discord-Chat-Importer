using System.Collections.Generic;
using Discord;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// Manages ChatImporters per channel, with max limit
	/// </summary>
	public class ChatImportManager : IChatImportManager
	{
		public Dictionary<IChannel, ChatImporter> Importers { get; }
		public int MaxSimultaneousImports { get; }
		public bool HasMaxImporters { get { return this.Importers.Count >= this.MaxSimultaneousImports; } }

		public ChatImportManager(int maxSimultaneousImports, Dictionary<IChannel, ChatImporter> existingImporters = null)
		{
			this.MaxSimultaneousImports = maxSimultaneousImports;

			if (existingImporters != null)
				existingImporters.Clear();

			this.Importers = existingImporters ?? new Dictionary<IChannel, ChatImporter>();
		}

		public bool ChannelHasImporter(IChannel channel)
		{
			return this.Importers.ContainsKey(channel);
		}

		public ChatImporter AddImporter(IChannel channel)
		{
			if (this.ChannelHasImporter(channel) || this.HasMaxImporters) 
				return this.GetImporter(channel);

			var newImporter = new ChatImporter();
			this.Importers.Add(channel, newImporter);
			return newImporter;
		}

		public void RemoveImporter(IChannel channel)
		{
			this.Importers.Remove(channel);
		}

		public ChatImporter GetImporter(IChannel channel)
		{
			return this.Importers[channel];
		}
	}
}
