using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class UndoCommand : CommandModule
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
		[Command("importer undo", true, RunMode = RunMode.Async)]
		public async Task RemoveImportedMessages(string channel = null)
		{
			await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.InvalidChannel));
		}
		
		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer undo", true, RunMode = RunMode.Async)]
		public async Task RemoveImportedMessages(ISocketMessageChannel channel = null)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.InvalidChannel));
				return;
			}

			// Confirmation
			IUserMessage reactMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed
			(
				"Import Deletion Confirmation",
				$@"Are you sure you want to do this? __ALL__ imported messages I've ever made for this channel **will be removed** and there is **no way to cancel**.",
				Color.Orange)
			);

			await this.Context.Bot.WaitForMessageReactionsAsync(this.Context.Guild, reactMsg, new List<string>() { "✅", "❌" }, DefaultPermission, true, async (SocketReaction reaction) => 
			{
				if (reaction.Emote.Name == "✅") // Go through entire channel and remove any messages made by us
				{
					await reactMsg.DeleteAsync();

					var messages = await channel.GetMessagesAsync(int.MaxValue).FlattenAsync();

					await Task.Run(async () =>
					{
						foreach (IMessage msg in messages)
						{
							if (msg.Author.Id == this.Context.Client.CurrentUser.Id)
							{
								await msg.DeleteAsync();
								await Task.Delay(200);
							}	
						}
					});

					await ReplyAsync(this.Context.User.Mention + " Deleted all imports. :thumbsup:");
				}
				else if (reaction.Emote.Name == "❌") // Cancel
				{
					await reactMsg.DeleteAsync();
				}
			});
		}
	}
}
