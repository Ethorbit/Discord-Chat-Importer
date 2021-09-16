using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Discord_Channel_Importer
{
	class Program
	{
		private DiscordSocketClient _client {get; set;}

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
				string token = (string)args.GetValue(0);

#if DEBUG
				Console.WriteLine("The token you entered: " + token);
#endif

				// Connect to Discord
				_client = new DiscordSocketClient();

				try // Do a login attempt
				{
					await _client.LoginAsync(Discord.TokenType.Bot, token);

					await Task.Delay(-1); // Keep the program running..
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error logging in with the provided token, did you enter it correctly? - ({e.Message})");
				}
			}
		}
	}
}
