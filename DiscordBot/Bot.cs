using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;

namespace Discord_Channel_Importer.DiscordBot
{
	public class Bot
	{
		private DiscordSocketClient _client { get; }

		public event EventHandler<DiscordBot.Log> Log;
		public event EventHandler<EventArgs> Logged_In;

		public DiscordBot.Settings Settings;

		private void MakeLog(string log)
		{
			Log.Invoke(this, new DiscordBot.Log() { Message = log });
		}

		public Bot(DiscordBot.Settings settings)
		{
			this.Settings = settings;
		}

		public async Task StartAsync()
		{
			this.Log += Bot_Log;

			// Connect
			await this.Settings.Client.LoginAsync(Discord.TokenType.Bot, Settings.Token);
			await this.Settings.Client.StartAsync();

			if (Logged_In != null)
				Logged_In.Invoke(this, null);

			// Create command
			//this.Settings.CommandService.AddModuleAsync();
		}

		private void Bot_Log(object sender, Log e) {}
	}
}
