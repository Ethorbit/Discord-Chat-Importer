using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace Discord_Channel_Importer.DiscordBot
{
	public class Bot
	{
		public event EventHandler<EventArgs> Logged_In;

		public readonly DiscordBot.Settings.BotSettings Settings;

		public Bot(DiscordBot.Settings.BotSettings settings)
		{
			this.Settings = settings;
		}

		/// <summary>
		/// Starts the bot up.
		/// </summary>
		/// <returns></returns>
		public async Task StartAsync()
		{
			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}

		/// <summary>
		/// Logs available commands
		/// </summary>
		public async Task LogCommands(BotSocketCommandContext cmdContext)
		{
			var channel = cmdContext.Channel;

			var embedBuilder = new EmbedBuilder();
			embedBuilder.Title = "Importer Commands";
			embedBuilder.Color = new Color(255, 255, 255);
			embedBuilder.AddField("import <url> #channel-name", @"Gets all Discord messages from the 
																provided URL (which should be a .json 
																with the proper Discord channel structure) 
																and recreates them in the provided channel.");
			embedBuilder.AddField("undo #channel-name", @"Removes ALL archived messages we made from the channel.");
			
			var embed = embedBuilder.Build();
			await channel.SendMessageAsync(null, false, embed);
		}

		/// <summary>
		/// Parses a Discord channel's json and generates embeds based off it, 
		/// which it sends off to the provided channel.
		/// </summary>
		public async Task ImportMessagesFromURLToChannel(BotSocketCommandContext cmdContext, string url, IChannel toChannel)
		{
			var embedBuilder = new EmbedBuilder();

			if (url.Length <= 0)
			{
				embedBuilder.Title = "URL required!";
				embedBuilder.Description = "How am I supposed to know what you want? Provide a URL to the json containing the channel with messages.";
				embedBuilder.Color = new Color(255, 0, 0);
				var embed = embedBuilder.Build();

				await cmdContext.Channel.SendMessageAsync(null, false, embed);
				return;
			}

			if (toChannel == null)
			{
				embedBuilder.Title = "Channel required!";
				embedBuilder.Description = "You want to import messages into a channel, but what channel? You forgot to tag the channel.. (#my-channel)";
				embedBuilder.Color = new Color(255, 0, 0);
				var embed = embedBuilder.Build();

				await cmdContext.Channel.SendMessageAsync(null, false, embed);
				return;
			}

			await Task.CompletedTask;
		}

		/// <summary>
		/// Removes all messages we ever archived from the specified channel.
		/// </summary>
		public async Task RemoveArchivedMessagesFromChannel(BotSocketCommandContext cmdContext, IChannel channel)
		{
			await Task.CompletedTask;
		}
	}
}
