using Discord;

namespace Discord_Channel_Importer.DiscordBot.Factories
{ 
	internal static class DiscordFactory
	{
		/// <summary>
		/// Creates a Discord EmbedField object for use in Embeds.
		/// </summary>
		public static EmbedField CreateEmbedField(string row, string text, bool inline = false)
		{
			var embedFieldBuilder = new EmbedFieldBuilder();
			embedFieldBuilder.Name = row;
			embedFieldBuilder.Value = text;
			embedFieldBuilder.IsInline = inline;

			return embedFieldBuilder.Build();
		}

		/// <summary>
		/// Creates a Discord Embed Footer
		/// </summary>
		public static EmbedFooter CreateEmbedFooter(string text, string iconUrl = null)
		{
			var embedBuilder = new EmbedFooterBuilder();
			embedBuilder.Text = text;
			embedBuilder.IconUrl = iconUrl;

			return embedBuilder.Build();
		}

		/// <summary>
		/// Creates a Discord Embed Author
		/// </summary>
		public static EmbedAuthor CreateEmbedAuthor(string name, string iconUrl, string url = null)
		{
			var embedBuilder = new EmbedAuthorBuilder();
			embedBuilder.Name = name;
			embedBuilder.IconUrl = iconUrl;
			embedBuilder.Url = url;

			return embedBuilder.Build();
		}

		/// <summary>
		/// Creates a Discord Embed object for use in sending messages.
		/// </summary>
		public static Embed CreateEmbed(string title = null, string description = null, Color color = new Color(), EmbedField[] fields = null, string thumbnailUrl = null, EmbedFooter? footer = null, EmbedAuthor? author = null, string imageUrl = null)
		{
			var embedBuilder = new EmbedBuilder()
			{
				Title = title,
				Description = description,
				Color = color
			};
	
			if (fields != null)
			{
				foreach (EmbedField field in fields)
				{
					embedBuilder.AddField(field.Name, field.Value);
				}
			}

			embedBuilder.ThumbnailUrl = thumbnailUrl;

			if (footer != null) embedBuilder.Footer = new EmbedFooterBuilder() { Text = footer.Value.Text, IconUrl = footer.Value.IconUrl };
			if (author != null) embedBuilder.Author = new EmbedAuthorBuilder() { Name = author.Value.Name, IconUrl = author.Value.IconUrl, Url = author.Value.Url };

			embedBuilder.ImageUrl = imageUrl;

			return embedBuilder.Build(); 
		}

		/// <summary>
		/// Creates a message emote.
		/// </summary>
		public static Emoji CreateEmoji(string emote)
		{
			return new Emoji(emote);
		}
	}
}
