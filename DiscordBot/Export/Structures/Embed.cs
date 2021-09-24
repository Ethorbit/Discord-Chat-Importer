using Discord;
using System;

namespace Discord_Channel_Importer.DiscordBot.Export.Structures
{
	internal struct Embed
	{
		public string Url { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTimeOffset? Timestamp { get; set; }

		public Color? Color { get; set; }

		public EmbedAuthor? Author { get; set; }

		public EmbedFooter? Footer { get; set; }
	}
}
