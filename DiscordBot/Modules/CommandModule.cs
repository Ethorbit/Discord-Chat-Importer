using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Modules
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			await this.Context.Bot.LogCommands(this.Context);
		}

		[Command("importer import", RunMode = RunMode.Async)]
		public async Task ImportMessages(string url = "", IChannel channel = null)
		{
			await this.Context.Bot.ImportMessagesFromURLToChannel(this.Context, url, channel);
		}

		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task UndoMessages(IChannel channel)
		{
			await this.Context.Bot.RemoveArchivedMessagesFromChannel(this.Context, channel);
		}
	}
}
