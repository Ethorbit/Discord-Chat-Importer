using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Factories;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		protected const GuildPermission CommandPermissions = (GuildPermission.ManageMessages | GuildPermission.ManageChannels);




		[RequireUserPermissionWithError(CommandPermissions, Group = "Permission")]
		[Command("importer cancel", RunMode = RunMode.Async)]
		public async Task CancelMessageImport(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Invalid Channel!", "What channel do you want to make me cancel importing to?", Color.Red));
				return;
			}

			try
			{
				BotReturn res = await this.Context.Bot.CancelImportingToChannelAsync(channel);

				if (res == BotReturn.ImporterDoesntExist)
				{
					await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Not importing!", "I am not importing messages to that channel.", Color.Red));
					return;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		[RequireUserPermissionWithError(CommandPermissions, Group = "Permission")]
		[Command("importer undo", RunMode=RunMode.Async)]
		public async Task RemoveImportedMessages(ISocketMessageChannel channel)
		{
			if (channel == null)
			{
				await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Channel required!", "You want to remove all archived messages, but from what channel? You forgot to tag the channel.. (#my-channel)", Color.Red));
				return;
			}

			try 
			{
				BotReturn res = await this.Context.Bot.RemoveArchivedMessagesFromChannelAsync(channel);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}		
		}
	}
}
