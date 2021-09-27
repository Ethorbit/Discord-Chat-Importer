using System;
using System.Threading.Tasks;
using Discord_Channel_Importer.DiscordBot.Importing;
using Discord_Channel_Importer.DiscordBot.Settings;
using Discord.WebSocket;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Commands;
using Discord;
using System.Collections.Generic;
using Discord_Channel_Importer.DiscordBot.Factories;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
	internal class Bot : IBot
	{
		public event EventHandler<EventArgs> Logged_In;
		public BotSettings Settings { get; }
		public ChatImportManager ChatImportManager { get; }

		/// <summary>
		/// Creates our custom Discord bot.
		/// </summary>
		public static async Task<Bot> CreateBot(string botToken)
		{
			var socketConfig = new DiscordSocketConfig();
			var commandServiceConfig = new CommandServiceConfig();

#if DEBUG
			socketConfig.LogLevel = Discord.LogSeverity.Debug;
#else
			socketConfig.LogLevel = Discord.LogSeverity.Error;
#endif

			var bot = new Bot(new BotSettings(new DiscordSocketClient(socketConfig), botToken), new ChatImportManager());

			var commandHandler = new CommandHandler(bot, new CommandService());
			await commandHandler.StartAsync();

			return bot;
		}

		public Bot(BotSettings settings, ChatImportManager importManager)
		{
			this.Settings = settings;
			this.ChatImportManager = importManager;
			this.ChatImportManager.StartImportLoop();
		}

		public async Task StartAsync()
		{	
			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}

		public async Task WaitForMessageReactionsAsync(SocketGuild guild, IUserMessage targetMessage, List<string> emotesAllowed, GuildPermission permissionsRequired, bool botShouldReact, Action<SocketReaction> callback)
		{
			if (botShouldReact)
			{
				await Task.Run(async () =>
				{
					foreach (string emoteName in emotesAllowed)
					{
						var emote = DiscordFactory.CreateEmoji(emoteName);
						await targetMessage.AddReactionAsync(emote);
					}
				});
			}

			this.Settings.Client.ReactionAdded += async (Cacheable<IUserMessage, ulong> reactMsg, ISocketMessageChannel reactChannel, SocketReaction reaction) =>
			{
				if (reactMsg.Id == targetMessage.Id && targetMessage.Channel == reactChannel)
				{
					bool isCorrectEmote = await Task.Run(() => { return emotesAllowed.Contains(reaction.Emote.Name); });

					if (isCorrectEmote)
					{
						var user = (IGuildUser)guild.GetUser(reaction.UserId);
						if (user.IsBot) return; 

						if (user.GuildPermissions.Has(permissionsRequired))
							callback(reaction);		
					}
				}
			};
		}
	}
}
