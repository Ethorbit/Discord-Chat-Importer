using Discord;

namespace Discord_Channel_Importer.DiscordBot.Factories
{ 
	public static class MessageFactory
	{
		/// <summary>
		/// Creates a Discord EmbedField object for use in Embeds.
		/// </summary>
		public static EmbedField CreateEmbedField(string row, string text)
		{
			var embedFieldBuilder = new EmbedFieldBuilder();
			embedFieldBuilder.Name = row;
			embedFieldBuilder.Value = text;

			return embedFieldBuilder.Build();
		}

		/// <summary>
		/// Creates a Discord Embed object for use in sending messages.
		/// </summary>
		public static Embed CreateEmbed(string title = null, string description = null, Color color = new Color(), EmbedField[] fields = null)
		{
			var embedBuilder = new EmbedBuilder();
			embedBuilder.Title = title;
			embedBuilder.Description = description;
			embedBuilder.Color = color;

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
