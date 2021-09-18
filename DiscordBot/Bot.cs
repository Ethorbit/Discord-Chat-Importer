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
		public event EventHandler<DiscordBot.Log> Log;
		public event EventHandler<EventArgs> Logged_In;

		protected readonly DiscordBot.Settings.BotSettings Settings;

		public Bot(DiscordBot.Settings.BotSettings settings)
		{
			this.Settings = settings;
		}
		protected void MakeLog(string log)
		{
			this.Log(this, new DiscordBot.Log() { Message = log });
		}

		/// <summary>
		/// Starts the bot up.
		/// </summary>
		/// <returns></returns>
		public async Task StartAsync()
		{
			this.Log += Bot_Log;
			this.Settings.Client.MessageReceived += Bot_MessageReceived;
			this.Settings.EmbedBuilder = this.Settings.EmbedBuilder ?? new EmbedBuilder();

			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, this.Settings.Token);
			await this.Settings.Client.StartAsync();

			if (this.Logged_In != null)
				this.Logged_In(this, null);
		}

		/// <summary>
		/// Imports a parsed json channel into an existing channel.
		/// </summary>
		/// <param name="URL"></param>
		/// <param name="Channel"></param>
		/// <returns></returns>
		public async Task ImportChannelAsync(string URL, string Channel)
		{
			
		}

		/// <summary>
		/// Removes our imported stuff from a channel
		/// </summary>
		/// <param name="Channel"></param>
		/// <returns></returns>
		public async Task RemoveImportsAsync(string Channel) 
		{

		}

		/// <summary>
		/// User sent a message in a channel visible to this bot
		/// </summary>
		private async Task Bot_MessageReceived(SocketMessage arg)
		{
			string message = arg.Content;
			
			if (message[0] == '!') // ! prefix is for our commands
			{
				// Get command and its arguments
				List<string> command_args = new List<string>();
				await Task.Run(() => command_args.AddRange(arg.Content.ToLower().Split(null)));
				int command_args_count = await Task.Run(() => { return command_args.Count(); }); 

				if (command_args_count < 2) // It needs to have the !importer as well as the subcommand name (import, eg. !importer import)
				{
					this.Settings.EmbedBuilder.Title = "Commands";
					this.Settings.EmbedBuilder.Description = "This is an embed test.";
					var embed = this.Settings.EmbedBuilder.Build();
					arg.Channel.SendMessageAsync("", false, embed);

					return; 
				}

				// Do command stuff
				string command = await Task.Run(() => { return command_args.First(); });	

				if (command == "!importer")
				{
					string subCommand = command_args[1];

					if (subCommand == "import")
					{
						if (command_args_count < 3) return;
						string url = command_args[2];
						string channel = command_args[3];

						MakeLog("url: " + url + " channel: " + channel);
						//arg.MentionedChannels;

						this.ImportChannelAsync(url, channel);
					}
					else if (subCommand == "undo")
					{
						if (command_args_count < 2) return;
						string channel = command_args[2];

						this.RemoveImportsAsync(channel);
					}
				}
			}
		}
		private void Bot_Log(object sender, Log e) {} 
	}
}
