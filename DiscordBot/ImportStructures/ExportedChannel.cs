using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.ImportStructures
{
	/// <summary>
	///	Contains the exported stuff necessary for making archives
	/// </summary>
	public class ExportedChannel
	{
		public Guild Guild { get; set; }
		public Channel Channel { get; set; }
		public List<Message> Messages { get; set; } 
	}
}
