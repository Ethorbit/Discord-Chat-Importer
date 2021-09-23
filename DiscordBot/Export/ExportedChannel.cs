using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.ImportStructures
{
	/// <summary>
	///	Contains the exported stuff necessary for making archives
	/// </summary>
	public struct ExportedChannel
	{
		public Guild Guild { get; set; }
		public Channel Channel { get; set; }
		public Stack<Message> Messages { get; set; } 
	}
}
