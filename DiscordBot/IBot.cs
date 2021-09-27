using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
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
		/// Do something after the provided message has been reacted to (with one of the specified emotes by someone who has the required permissions)
		/// </summary>
		/// <param name="targetMessage">Message to check reactions for.</param>
		/// <param name="emotesAllowed">The emotes to check from the reactions.</param>
		/// <param name="permissionsRequired">The user permissions required for the reaction to be checked.</param>
		/// <param name="botShouldReact">Whether or not the bot should automatically add the specified Emotes.</param>
		/// <param name="callback">Custom code to run when all conditions have been met, containing the SocketReaction object.</param>
		Task WaitForMessageReactionsAsync(SocketGuild guild, IUserMessage targetMessage, List<string> emotesAllowed, GuildPermission permissionsRequired, bool botShouldReact, Action<SocketReaction> callback);
	}
}
