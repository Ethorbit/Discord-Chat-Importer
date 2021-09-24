using Discord;

namespace Discord_Channel_Importer.DiscordBot.Factories
{ 
	internal static class MessageFactory
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
		/// Creates a Discord Embed object for use in sending messages.
		/// </summary>
		public static Embed CreateEmbed(string title = null, string description = null, Color color = new Color(), EmbedField[] fields = null)
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

			return embedBuilder.Build(); 
		}
	}
}
