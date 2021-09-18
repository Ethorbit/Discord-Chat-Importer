using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;

namespace Discord_Channel_Importer
{
	class Program
	{
		static void Main(string[] args)
			=> new Program().MainAsync(args).GetAwaiter().GetResult();

		private async Task MainAsync(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Enter the bot's token:");
				string token = Console.ReadLine();
				args = new string[] { token };
			}

			if (args.Length >= 1)
			{
				// Create Discord bot
				string token = (string)args.GetValue(0);
				var discordBot = await DiscordBot.Factories.BotFactory.CreateBot(token);
				discordBot.Logged_In += async (object sender, EventArgs e) => Task.Run(() => Console.WriteLine("Logged in."));
				
				Task.Run(() => Console.WriteLine("Logging in..."));

				try // Attempt to start our Discord bot
				{
					await discordBot.StartAsync(); 

					await Task.Delay(-1); // Keep the program running.
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error starting bot, did you enter the token correctly? - ({e.Message})");
				}
			}
		}
	}
}