using System;
using System.Threading.Tasks;
using Discord;
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
		Task StartAsync();

		/// <summary>
		/// Parses a Discord channel's json and generates embeds based off it, 
		/// which it sends off to the provided channel.
		/// </summary>
		Task<BotReturn> ImportMessagesFromURIToChannelAsync(Uri uri, IChannel channel);

		/// <summary>
		/// Cancels importing to a channel that we're currently making archived messages in
		/// </summary>
		Task<BotReturn> CancelImportingToChannelAsync(IChannel channel);

		/// <summary>
		/// Removes all messages we ever archived from the specified channel.
		/// </summary>
		Task<BotReturn> RemoveArchivedMessagesFromChannelAsync(IChannel channel);
	}

}
