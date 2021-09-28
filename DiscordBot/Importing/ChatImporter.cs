using Discord;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.DiscordBot.Export.Structures;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	/// <summary>
	/// A Discord channel chat importer
	/// </summary>
	internal class ChatImporter : IChatImporter
	{
		public event EventHandler<IChatImporterSettings> FinishImports;
		public IChatImporterSettings Settings { get; }
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
			DateTime msgTime = msg.Timestamp.UtcDateTime;
			var author = DiscordFactory.CreateEmbedAuthor(msg.Author.Name, msg.Author.AvatarUrl);
			var footer = DiscordFactory.CreateEmbedFooter($"(Archived from: {msgTime.ToString("d")} at {msgTime.ToString("t")} UTC)");

			if (msg.Content.Length > 0) // Regular message text
			{
				await this.Settings.Destination.SendMessageAsync(null, false, DiscordFactory.CreateEmbed(null, msg.Content, Color.Gold, null, null, footer, author));
			}

			//// Commented out because bots cannot create embeds with video players and stuff, so there's no sense trying to recreate them
			//if (msg.Embeds.Count > 0) // Each URL generates separate embeds, we recreate those here
			//{
			//	await Task.Run(async () =>
			//	{
			//		foreach (Export.Structures.Embed embed in msg.Embeds)
			//		{
			//			var embedFields = new EmbedField[embed.Fields.Length];

			//			foreach (Field field in embed.Fields)
			//			{
			//				embedFields[embedFields.Length] = DiscordFactory.CreateEmbedField(field.Name, field.Value, field.IsInline);
			//			}

			//			Color? embedColor = null;

			//			try
			//			{
			//				embedColor = (Color)System.Drawing.ColorTranslator.FromHtml(embed.Color);
			//			}
			//			finally
			//			{
			//				await this.Settings.Destination.SendMessageAsync(
			//					null,
			//					false,
			//					DiscordFactory.CreateEmbed
			//					(
			//						embed.Title,
			//						embed.Description,
			//						embedColor.HasValue ? embedColor.Value : default,
			//						embedFields.Length > 0 ? embedFields : null,
			//						embed.Thumbnail.HasValue ? embed.Thumbnail.Value.Url : null,
			//						embed.Footer.HasValue ? (EmbedFooter?)DiscordFactory.CreateEmbedFooter(embed.Footer.Value.Text, embed.Footer.Value.IconUrl) : null,
			//						embed.Author.HasValue ? (EmbedAuthor?)DiscordFactory.CreateEmbedAuthor(embed.Author.Value.Name, embed.Author.Value.IconUrl, embed.Author.Value.Url) : null
			//					)
			//				);
			//			}
			//		}
			//	});
			//}
			
			if (msg.Attachments.Count > 0) // Each attachment uploaded from a device, though they will only be displayed in their URL form here
			{
				await Task.Run(async () => 
				{
					foreach (Export.Structures.Attachment att in msg.Attachments)
					{
						await this.Settings.Destination.SendMessageAsync(null, false, DiscordFactory.CreateEmbed(null, att.Url, Color.Gold, null, null, footer, author));
					}
				});
			}

			expChan.Messages.Dequeue();
		}
	}
}
