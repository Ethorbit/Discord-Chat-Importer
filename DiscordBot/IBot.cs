using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.DiscordBot.Settings;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our custom Discord bot
	/// </summary>
	internal interface IBot
	{
		BotSettings Settings { get; }
		ChatImportManager ChatImportManager { get; }

		/// <summary>
		/// Starts the bot up.
		/// </summary>
		/// <returns></returns>
		Task StartAsync();
	}
}
