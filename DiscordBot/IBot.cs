using System;
using System.Threading.Tasks;
using Discord.WebSocket;
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

		/// <summary>
		/// Parses a Discord channel's json and generates embeds based off it, 
		/// which it creates and returns as a ChatImporter object.
		/// </summary>
		Task<BotReturn> GetChatImporterFromUriAsync(Uri uri, ISocketMessageChannel channel, Action<IChatImporter> callback);

		/// <summary>
		/// Uses the provided ChatImporter on the channel
		/// </summary>
		//public void ImportMessagesToChannel(ChatImporter importer, ISocketMessageChannel channel);

		/// <summary>
		/// Cancels importing to a channel that we're currently making archived messages in
		/// </summary>
		Task<BotReturn> CancelImportingToChannelAsync(ISocketMessageChannel channel);

		/// <summary>
		/// Removes all messages we ever archived from the specified channel.
		/// </summary>
		Task<BotReturn> RemoveArchivedMessagesFromChannelAsync(ISocketMessageChannel channel);
	}

}
