using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ChatImportManagerEventArgs
	{
		public IChatImporter Importer { get; }
		public ISocketMessageChannel Channel { get; }

		public ChatImportManagerEventArgs(ISocketMessageChannel channel, IChatImporter importer)
		{
			this.Importer = importer;
			this.Channel = channel;
		}
	}
}
