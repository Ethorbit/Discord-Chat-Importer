using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

		/// <summary>
		/// The time it takes for our ImportLoop to iterate each stored Importer
		/// </summary>
		public int TimeForEachImporter { get; set; } = 500;
		public Dictionary<IChannel, IChatImporter> Importers { get; } = new Dictionary<IChannel, IChatImporter>();
		private CancellationTokenSource _importCancellationSource { get; set; } 

		/// <summary>
		/// Starts automatically using our Importers to import stuff
		/// </summary>
		public void StartImportLoopAsync()
		{
			if (this._importCancellationSource == null || _importCancellationSource.IsCancellationRequested)
			{
				_importCancellationSource = new CancellationTokenSource();
				var cancellationToken = _importCancellationSource.Token;

				Thread tImportLoop = new Thread(new ThreadStart(async () => {
					while (!cancellationToken.IsCancellationRequested)
					{
						foreach (IChatImporter importer in this.Importers.Values)
						{					
							await Task.Delay(this.TimeForEachImporter);
							await importer.ImportNextMessage();
						}
					}
				}));

				tImportLoop.Start();
			}
		}

		/// <summary>
		/// Stops automatically importing
		/// </summary>
		public void StopImportLoop()
		{
			_importCancellationSource.Cancel();
		}

		public bool ChannelHasImporter(ISocketMessageChannel channel)
		{
			return this.Importers.ContainsKey(channel);
		}

		/// <summary>
		///	Adds another importer to the Importer Stack and will start using it.
		/// </summary>
		/// <returns>The IChatImporter, null if it couldn't be added.</returns>
		public IChatImporter AddImporter(ISocketMessageChannel channel, ExportedChannel exportedChannel, IChatImporter importer = null)
		{
			if (this.ChannelHasImporter(channel)) 
				return this.GetImporter(channel);

			importer = importer ?? new ChatImporter(channel, exportedChannel);
			this.Importers.Add(channel, importer);

			importer.FinishImports += Importer_FinishImports;

			return importer;
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

		/// <summary>
		/// Gets the estimated time when an Importer will be finished (this will immediately be wrong if a new Importer is added)
		/// </summary>
		public TimeSpan GetEstimatedImportTime(ISocketMessageChannel channel)
		{
			var importer = this.GetImporter(channel);
			return TimeSpan.FromSeconds((this.TimeForEachImporter * this.Importers.Count) * importer.Source.Messages.Count);
		}

		/// <summary>
		/// Gets the estimated time when all Importers will be finished (this will immediately be wrong if a new Importer is added)
		/// </summary>
		/// <returns></returns>
		public TimeSpan GetEstimatedImportTime()
		{
			int total_messages = 0;

			foreach (IChatImporter importer in this.Importers.Values)
			{
				total_messages += importer.Source.Messages.Count;
			}

			return TimeSpan.FromSeconds((this.TimeForEachImporter * this.Importers.Count) * total_messages);
		}
		
		/// <summary>
		/// When one of our Importers has completed
		/// </summary>
		private void Importer_FinishImports(object sender, ChatImporterEventArgs e)
		{
			if (sender is IChatImporter importer)
			{
				this.Importers.Remove(e.Channel);
				this.ImportFinished(this, new ChatImportManagerEventArgs(e.Channel, importer));
			}
		}
	}
}
