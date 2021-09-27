using Discord_Channel_Importer.DiscordBot.Export.Structures;
using Discord_Channel_Importer.Utilities;
using System;
using System.Collections.Generic;
namespace Discord_Channel_Importer.DiscordBot.Export
{
	/// <summary>
	///	Contains the exported stuff necessary for making archives
	/// </summary>
	internal class ExportedChannel
	{
		public Guild Guild { get; set; }
		public Channel Channel { get; set; }
		public Queue<Message> Messages { get; set; } 
	}
}
