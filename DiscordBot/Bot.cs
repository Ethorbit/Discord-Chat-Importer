using System;
using System.Threading.Tasks;
using Discord;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.Utilities;

namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Our Custom Discord bot.
	/// </summary>
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

			var embed = MessageFactory.CreateEmbed("Importer Commands", null, Color.LightGrey, new EmbedField[] {
				MessageFactory.CreateEmbedField("import <url> #channel-name", @"Gets all Discord messages from the 
																provided URL (which should be a .json 
																with the proper Discord channel structure) 
																and recreates them in the provided channel."),
				MessageFactory.CreateEmbedField("undo #channel-name", "Removes ALL archived messages we made in the specified channel.")
			}); 

			await channel.SendMessageAsync(null, false, embed);
		}

		/// <summary>
		/// Parses a Discord channel's json and generates embeds based off it, 
		/// which it sends off to the provided channel.
		/// </summary>
		public async Task ImportMessagesFromURLToChannel(BotSocketCommandContext cmdContext, string url, IChannel toChannel)
		{
			if (url.Length <= 0)
			{
				await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("URL required!", "How am I supposed to know what you want? Provide a URL to the json containing the channel with messages.", Color.Red));
				return;
			}

			if (toChannel == null)
			{
				await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Channel required!", "You want to import messages into a channel, but what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			// TODO: Validate URL, error out if it's not valid
			await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Please wait...", "I am downloading the text from that URL, this could take some time.", Color.LightGrey));
			
			await JsonFactory.CreateFromURL("https://www.dropbox.com/s/m5gmfve6xw7yf55/chronicles-changes.json?dl=0&raw=1");
			await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Success!", "I finished the request.", Color.Green));
		}

		/// <summary>
		/// Removes all messages we ever archived from the specified channel.
		/// </summary>
		public async Task RemoveArchivedMessagesFromChannel(BotSocketCommandContext cmdContext, IChannel channel)
		{
			if (channel == null)
			{
				await cmdContext.Channel.SendMessageAsync(null, false, MessageFactory.CreateEmbed("Channel required!", "You want to remove all archived messages, but from what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			await Task.CompletedTask;
		}
	}
}
