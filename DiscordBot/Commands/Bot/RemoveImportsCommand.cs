using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class RemoveImportsCommand : CommandModule
	{
		public override CommandInfo[] CommandInfo { get; } = new CommandInfo[]
		{
			new CommandInfo()
			{
				Usage = "undo #channel-name",
				Description = "Removes ALL archived messages I made in the specified channel and automatically cancels importing to that channel."
			}
		};

		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer undo", RunMode = RunMode.Async)]
		public async Task RemoveImportedMessages(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Channel required!", "You want to remove all archived messages, but from what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			// TODO: go through entire channel and remove any messages made by us
		}
	}
}
