using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// Manages IChatImporters, with a custom max allowed limit and import delays
	/// </summary>
	internal class ChatImportManager
	{
		public event EventHandler<ChatImportManagerEventArgs> ImportFinished;

		public Dictionary<IChannel, IChatImporter> Importers { get; }
		public ChatImportManagerSettings Settings { get; }
		public bool HasMaxImporters { get { return this.Importers.Count >= this.Settings.MaxSimultaneousImports; } }

		public ChatImportManager(ChatImportManagerSettings settings, Dictionary<IChannel, IChatImporter> existingImporters = null)
		{
			this.Settings = settings;

			if (existingImporters != null)
				existingImporters.Clear();

			this.Importers = existingImporters ?? new Dictionary<IChannel, IChatImporter>();
		}

		public bool ChannelHasImporter(ISocketMessageChannel channel)
		{
			return this.Importers.ContainsKey(channel);
		}

		public void SetImporterIntervals(double delay)
		{
			int i = 1;

			foreach (IChatImporter importer in this.Importers.Values)
			{
				importer.ImportTimer.Interval = delay + i;

				if (importer.ImportTimer.Enabled)
				{
					importer.ImportTimer.Stop();
					importer.ImportTimer.Start();
				}

				i++;
			}
		}

		/// <summary>
		///	Adds another importer to the Importer Stack
		/// </summary>
		/// <returns>The IChatImporter, null if it couldn't be added.</returns>
		public IChatImporter AddImporter(ISocketMessageChannel channel, ExportedChannel exportedChannel, IChatImporter importer = null)
		{
			if (this.HasMaxImporters)
				return null;

			if (this.ChannelHasImporter(channel)) 
				return this.GetImporter(channel);

			importer = importer ?? new ChatImporter(new ChatImporterSettings(channel, exportedChannel));
			this.Importers.Add(channel, importer);
			this.SetImporterIntervals(this.Settings.ImportTime + (this.Settings.AddedTimeForEachImporter * this.Importers.Count));

			importer.FinishImports += Importer_FinishImports;

			return importer;
		}

		private void Importer_FinishImports(object sender, ChatImporterEventArgs e)
		{
			if (sender is IChatImporter importer)
			{
				this.Importers.Remove(e.Channel);
				this.ImportFinished(this, new ChatImportManagerEventArgs(e.Channel, importer));
			}	
		}

		/// <summary>
		/// Removes the Importer assigned to the specified channel from the Import Stack
		/// </summary>
		/// <returns>
		/// false if there was no Importer for that channel.
		/// </returns>
		public bool RemoveImporter(ISocketMessageChannel channel)
		{
			return this.Importers.Remove(channel);
		}

		/// <summary>
		/// Gets the Importer that's assigned to the specified channel
		/// </summary>
		/// <returns>null if the channel had no Importer.</returns>
		public IChatImporter GetImporter(ISocketMessageChannel channel)
		{
			return this.Importers[channel];
		}
	}
}
