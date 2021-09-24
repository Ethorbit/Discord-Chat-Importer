using Discord_Channel_Importer.DiscordBot.Export.Structures;
using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.Export
{
	/// <summary>
	///	Contains the exported stuff necessary for making archives
	/// </summary>
	internal struct ExportedChannel
	{
		public Guild Guild { get; set; }
		public Channel Channel { get; set; }
		public Stack<Message> Messages { get; set; } 
	}
}
