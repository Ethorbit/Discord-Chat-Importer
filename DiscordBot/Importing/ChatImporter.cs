using Discord;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Export.Structures;
using System;
using System.Timers;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// A Discord channel chat importer
	/// </summary>
	internal class ChatImporter : IChatImporter
	{
		public event EventHandler<IChatImporterSettings> FinishImports;
		public IChatImporterSettings Settings { get; }
		public bool IsEnabled { get; set; } = false;
		public bool IsFinished { get; private set; }

		public ChatImporter(IChatImporterSettings settings)
		{
			this.Settings = settings;
		}
		
		public async Task ImportNextMessage()
		{
			var expChan = this.Settings.Source;

			if (this.IsFinished || expChan.Messages.Count <= 0)
			{
				this.IsFinished = true;

				if (this.FinishImports != null)
					this.FinishImports(this, this.Settings);

				return;
			}

			Message msg = expChan.Messages.Peek();

			Discord.Embed embed = DiscordFactory.CreateEmbed(msg.Author.Name, msg.Content, Color.Gold);
			await this.Settings.Destination.SendMessageAsync(null, false, embed);

			expChan.Messages.Pop();
		}
	}
}
