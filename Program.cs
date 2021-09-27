using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer
{
	internal class Program
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

				var discordBot = await DiscordBot.Bot.CreateBot(token);
				discordBot.Logged_In += async (object sender, EventArgs e) => await Task.Run(() => Console.WriteLine("Logged in."));	
				await Task.Run(() => Console.WriteLine("Logging in..."));

				try // Attempt to start our Discord bot
				{
					await discordBot.StartAsync(); 

					await Task.Delay(-1); // Keep the program running.
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error starting bot. Did you enter the token correctly? - ({e.Message})");
				}
			}
		}
	}
}