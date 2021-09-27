using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class CancelCommand : CommandModule
	{
		public override CommandInfo[] CommandInfo { get; } = new CommandInfo[] {
			new CommandInfo() {
				Usage = "cancel #channel-name",
				Description = "Cancels importing to a channel."
			},
			new CommandInfo() {
				Usage = "cancel all",
				Description = "Cancels all of our channel imports."
			}
		};

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
			await ReplyAsync(this.Context.User.Mention + $" Stopped importing to {channel.Name}.");
		}

		[RequireUserPermissionWithError(DefaultPermission, Group = "Permission")]
		[Command("importer cancel all", RunMode = RunMode.Async)]
		public async Task CancelAllMessageImports()
		{
			if (!this.Context.Bot.ChatImportManager.ChannelHasImporter(this.Context.Channel))
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Not importing!", "I am not importing messages to that channel.", Color.Red));
				return;
			}

			this.Context.Bot.ChatImportManager.StopImportLoop(); // Pause the imports while the confirmation shows

			IUserMessage reactMsg = await ReplyAsync(null, false, DiscordFactory.CreateEmbed
			(
				"Cancel Confirmation",
				$@"You are about to cancel ALL {Context.Bot.ChatImportManager.Importers.Count} imports, are you sure?",
				Color.Orange)
			);

			await this.Context.Bot.WaitForMessageReactionsAsync(this.Context.Guild, reactMsg, new List<string>() { "✅", "❌" }, DefaultPermission, true, async (SocketReaction reaction) =>
			{
				if (reaction.Emote.Name == "✅") // Proceed 
				{
					await reactMsg.DeleteAsync();

					this.Context.Bot.ChatImportManager.ClearImporters();

					await ReplyAsync(reaction.User.Value.Mention + " Cleared all Importers.");
				}
				else if (reaction.Emote.Name == "❌") // Cancel
				{
					await reactMsg.DeleteAsync();
					this.Context.Bot.ChatImportManager.StartImportLoop();
				}
			});

			// If the confirmation message is not handled within a certain amount of time, continue the importing again
			await Task.Delay(20000);

			if (reactMsg != null)
			{
				await reactMsg.DeleteAsync();
				this.Context.Bot.ChatImportManager.StartImportLoop();
			}
		}
	}
}
