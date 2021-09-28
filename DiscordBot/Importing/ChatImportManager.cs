using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// Stores and controls IChatImporters
	/// </summary>
	internal class ChatImportManager
	{
		public event EventHandler<ChatImportManagerEventArgs> ImportFinished = delegate { };

		/// <summary>
		/// The time (in milliseconds) it takes for our ImportLoop to iterate each stored Importer
		/// </summary>
		public double ImportLoopIterationTime { get; set; } = 500;
		public ConcurrentDictionary<IChannel, IChatImporter> Importers { get; } = new ConcurrentDictionary<IChannel, IChatImporter>();
		public bool IsImportLoopRunning { get { return !_importCancellationSource.IsCancellationRequested; } }
		private CancellationTokenSource _importCancellationSource { get; set; }
	
		/// <summary>
		/// Starts automatically using our Importers to import stuff
		/// </summary>
		public void StartImportLoop()
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
							if (importer.Settings.IsEnabled)
							{ 
								await Task.Delay((int)this.ImportLoopIterationTime, cancellationToken);
								await importer.ImportNextMessage();
							}
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
		public IChatImporter AddImporter(IChatImporter importer)
		{
			ISocketMessageChannel channel = importer.Settings.Destination;

			if (this.ChannelHasImporter(channel)) 
				return this.GetImporter(channel);

			this.Importers.TryAdd(channel, importer);

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
			return this.Importers.TryRemove(channel, out _);
		}
		/// <summary>
		/// Removes all assigned Importers.
		/// </summary>
		public void ClearImporters()
		{
			this.Importers.Clear();
		}

		/// <summary>
		/// Gets the Importer that's assigned to the specified channel
		/// </summary>
		/// <returns>null if the channel had no Importer.</returns>
		public IChatImporter GetImporter(ISocketMessageChannel channel)
		{
			IChatImporter importer;
			this.Importers.TryGetValue(channel, out importer);

			return importer;
		}

		/// <summary>
		/// Gets the estimated time when an Importer will be finished (this will immediately be wrong if a new Importer is added)
		/// </summary>
		public TimeSpan GetEstimatedImportTime(ISocketMessageChannel channel)
		{
			var importer = this.GetImporter(channel);
			
			if (importer == null) 
				return TimeSpan.Zero;

			// TODO: improve this, we don't know how long the importer count will remain, so the estimation can jump from 3 hours to 1.
			return TimeSpan.FromSeconds(((this.ImportLoopIterationTime / 1000) * this.Importers.Count) * importer.Settings.Source.Messages.Count);
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
				if (importer.Settings.IsEnabled)
					total_messages += importer.Settings.Source.Messages.Count;
			}

			// TODO: improve this, we don't know how long the importer count will remain, so the estimation can jump from 3 hours to 1.
			return TimeSpan.FromSeconds(((this.ImportLoopIterationTime / 1000) * this.Importers.Count) * total_messages);
		}
		
		/// <summary>
		/// When one of our Importers has completed
		/// </summary>
		private void Importer_FinishImports(object sender, IChatImporterSettings e)
		{
			if (sender is IChatImporter importer)
			{
				this.RemoveImporter(e.Destination);
				this.ImportFinished(this, new ChatImportManagerEventArgs(importer, e));
			}
		}
	}
}
