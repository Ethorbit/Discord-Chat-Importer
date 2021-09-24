using Discord;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Export.Structures;
using System;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// A Discord channel chat importer
	/// </summary>
	internal class ChatImporter : IChatImporter
	{
		public event EventHandler<ImportEventArgs> FinishImports;
		public Timer ImportTimer { get; }
		public IChatImporterSettings Settings { get; }
		public bool IsFinished { get; private set; }

		public ChatImporter(IChatImporterSettings settings)
		{
			this.Settings = settings;
			this.ImportTimer = settings.ImportTimer ?? new Timer();
			this.ImportTimer.AutoReset = true;
			this.ImportTimer.Elapsed += ImportTimer_Elapsed;
		}

		~ChatImporter()
		{
			this.StopImport();
		}

		public void StartImport()
		{
			this.Settings.ImportTimer.Start();
		}

		public void StopImport()
		{
			this.Settings.ImportTimer.Stop();
		}

		/// <summary>
		/// Timer executes endlessly, we import the messages here and self stop the timer when finished.
		/// </summary>
		private async void ImportTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var expChan = this.Settings.ExportedChannel;

			if (this.IsFinished || expChan.Messages.Count <= 0)
			{
				this.StopImport();
				this.IsFinished = true;

				if (this.FinishImports != null)
					this.FinishImports(this, new ImportEventArgs(this.Settings.ImportChannel));
			}

			Message msg = expChan.Messages.Peek();

			Discord.Embed embed = MessageFactory.CreateEmbed(msg.Author.Name, msg.Content, Color.Gold);
			await this.Settings.ImportChannel.SendMessageAsync(null, false, embed);

			expChan.Messages.Pop();
		}
	}
}
