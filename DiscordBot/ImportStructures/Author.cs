﻿namespace Discord_Channel_Importer.DiscordBot.ImportStructures
{
	public class Author
	{
		public string AvatarUrl { get; set; }

		public string Discriminator { get; set; }

		public bool IsBot { get; set; }

		public string Name { get; set; }

		public ulong Id { get; set; }
	}
}
