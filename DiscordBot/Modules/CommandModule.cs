using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Modules
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			try
			{
				await this.Context.Bot.LogCommands(this.Context);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", IChannel channel = null)
		{
			try
			{
				await this.Context.Bot.ImportMessagesFromURLToChannel(this.Context, url, channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task UndoMessages(IChannel channel)
		{
			try 
			{
				await this.Context.Bot.RemoveArchivedMessagesFromChannel(this.Context, channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}		
		}
	}
}
