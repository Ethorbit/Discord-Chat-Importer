using System;
using System.Collections.Generic;
namespace Discord_Channel_Importer.DiscordBot.ImportStructures
{
	public struct Message
	{
		public bool IsPinned { get; set; }

		public string Content { get; set; }

		public DateTimeOffset Timestamp { get; set; }

		public DateTimeOffset? TimestampEdited { get; set; }

		public Author Author { get; set; }

		public List<Attachment> Attachments { get; set; }

		public List<Embed> Embeds { get; set; }

		public ulong Id { get; set; }
	}
}
