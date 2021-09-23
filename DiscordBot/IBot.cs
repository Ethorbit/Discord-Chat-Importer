using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.DiscordBot.Settings;

namespace Discord_Channel_Importer.DiscordBot
{
	public interface IBot
	{
		public BotSettings Settings { get; }

		public ChatImportManager ChatImportManager { get; }


		/// <summary>
		/// Starts the bot up.
		/// </summary>
		/// <returns></returns>
		public Task StartAsync();

		/// <summary>
		/// Parses a Discord channel's json and generates embeds based off it, 
		/// which it creates and returns as a ChatImporter object.
		/// </summary>
		public Task<BotReturn> GetChatImporterFromUriAsync(Uri uri, ISocketMessageChannel channel, Action<ChatImporter> callback);

		/// <summary>
		/// Uses the provided ChatImporter on the channel
		/// </summary>
		//public void ImportMessagesToChannel(ChatImporter importer, ISocketMessageChannel channel);

		/// <summary>
		/// Cancels importing to a channel that we're currently making archived messages in
		/// </summary>
		public Task<BotReturn> CancelImportingToChannelAsync(ISocketMessageChannel channel);

		/// <summary>
		/// Removes all messages we ever archived from the specified channel.
		/// </summary>
		public Task<BotReturn> RemoveArchivedMessagesFromChannelAsync(ISocketMessageChannel channel);
	}

}
