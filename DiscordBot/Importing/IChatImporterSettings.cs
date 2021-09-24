using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Export;
using System.Timers;

namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal interface IChatImporterSettings
	{
		Timer ImportTimer { get; }

		ISocketMessageChannel ImportChannel { get; }

		ExportedChannel ExportedChannel { get; }
	}
}
