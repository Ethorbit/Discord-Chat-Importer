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
		public event EventHandler<ChatImporterEventArgs> FinishImports;
		public ISocketMessageChannel Destination { get; }
		public ExportedChannel Source { get; }
		public bool IsFinished { get; private set; }

		public ChatImporter(ISocketMessageChannel importChannel, ExportedChannel exportChannel)
		{
			this.Destination = importChannel;
			this.Source = exportChannel;
		}
		
		public async Task ImportNextMessage()
		{
			var expChan = this.Source;

			if (this.IsFinished || expChan.Messages.Count <= 0)
			{
				this.IsFinished = true;

				if (this.FinishImports != null)
					this.FinishImports(this, new ChatImporterEventArgs(this.Destination));

				return;
			}

			Message msg = expChan.Messages.Peek();

			Discord.Embed embed = DiscordFactory.CreateEmbed(msg.Author.Name, msg.Content, Color.Gold);
			await this.Destination.SendMessageAsync(null, false, embed);

			expChan.Messages.Pop();
		}
	}
}
