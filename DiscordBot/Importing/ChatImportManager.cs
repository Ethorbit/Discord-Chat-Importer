using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// Manages ChatImporters per channel, with a custom max allowed limit and auto import delays
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

		public bool ChannelHasImporter(ISocketMessageChannel channel)
		{
			return this.Importers.ContainsKey(channel);
		}

		public void ConfigureDelays(double delay)
		{
			foreach (ChatImporter importer in this.Importers.Values)
			{
				importer.Settings.ImportTimer.Interval = (delay * 1000);
			}
		}

		public ChatImporter AddImporter(ISocketMessageChannel channel, ExportedChannel export)
		{
			if (this.ChannelHasImporter(channel) || this.HasMaxImporters) 
				return this.GetImporter(channel);

			var newImporter = new ChatImporter(new ChatImporterSettings(channel, export));
			this.Importers.Add(channel, newImporter);
			return newImporter;
		}

		public void RemoveImporter(ISocketMessageChannel channel)
		{
			this.Importers.Remove(channel);
		}

		public ChatImporter GetImporter(ISocketMessageChannel channel)
		{
			return this.Importers[channel];
		}
	}
}
