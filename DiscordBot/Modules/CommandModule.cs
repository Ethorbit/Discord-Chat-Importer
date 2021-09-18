using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Modules
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			var bot = this.Context.Bot;
			await bot.LogCommands(this.Context);
		}

		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", IChannel channel = null)
		{
			await Context.Bot.ImportMessagesFromURLToChannel(Context, url, channel);
		}

		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task UndoMessages(IChannel channel)
		{
			await Context.Bot.RemoveArchivedMessagesFromChannel(Context, channel);
		}
	}
}
