using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class CancelCommand : CommandModule
	{
		public override string Usage { get; } = "cancel #channel-name";
		public override string Description { get; } = "Stops importing to a channel.";


		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer cancel", RunMode = RunMode.Async)]
		public async Task CancelMessageImport(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Invalid Channel!", "What channel do you want to make me cancel importing to?", Color.Red));
				return;
			}

			if (!this.Context.Bot.ChatImportManager.ChannelHasImporter(channel))
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Not importing!", "I am not importing messages to that channel.", Color.Red));
				return;
			}

			this.Context.Bot.ChatImportManager.RemoveImporter(channel);
		}
	}
}
