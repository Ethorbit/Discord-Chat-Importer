﻿namespace Discord_Channel_Importer.DiscordBot.Export.Structures
{
	public struct Attachment
	{
		public ulong Id { get; set; }

		public string Filename { get; set; }

		public string Url { get; set; }

		public int Filesizebytes { get; set; }
	}
}
