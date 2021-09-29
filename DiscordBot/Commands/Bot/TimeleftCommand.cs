using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Channel_Importer.DiscordBot.Commands.Modules;
using Discord_Channel_Importer.DiscordBot.Factories;
using Discord_Channel_Importer.Extensions;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Bot
{
	public class TimeleftCommand : CommandModule
	{
		public override CommandInfo[] CommandInfo { get; } = new CommandInfo[] {
			new CommandInfo() {
				Usage = "timeleft #channel-name",
				Description = "Returns an estimation of how much time is left for the specified channel's import."
			},
			new CommandInfo() {
				Usage = "timeleft all",
				Description = "Returns an estimation of how much time is left before all current imports are finished."
			}
		};

		[Command("importer timeleft", true, RunMode = RunMode.Async)]
		[Alias("importer time")]
		public async Task ShowTimeleft(string channel = null)
		{
			await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.InvalidChannel));
			await SendUsageAsync();
		}

		[Command("importer timeleft", true, RunMode = RunMode.Async)]
		[Alias("importer time")]
		public async Task ShowTimeleft(ISocketMessageChannel channel = null)
		{
			#region Attempt
			if (channel == null)
			{
				await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.InvalidChannel));
				await SendUsageAsync();
				return;
			}

			if (!this.Context.Bot.ChatImportManager.ChannelHasImporter(channel))
			{
				await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.NoImportForChannel));
				return;
			}
			#endregion
				 
			TimeSpan estimatedTime = this.Context.Bot.ChatImportManager.GetEstimatedImportTime(channel);
			await ReplyAsync(null, false, DiscordFactory.CreateEmbed($"Import Timeleft for {channel.Name}", estimatedTime.ToReadableFormat(), Color.LightGrey));
		}

		[Command("importer timeleft all", true, RunMode = RunMode.Async)]
		[Alias("importer time all")]
		public async Task ShowTimeleft()
		{
			if (this.Context.Bot.ChatImportManager.Importers.Count == 0)
			{
				await ReplyAsync(null, false, BotMessageFactory.CreateEmbed(BotMessageType.NoImports));
				return;
			}
				
			TimeSpan estimatedTime = this.Context.Bot.ChatImportManager.GetEstimatedImportTime();
			await ReplyAsync(null, false, DiscordFactory.CreateEmbed($"Total Import Timeleft", estimatedTime.ToReadableFormat(), Color.LightGrey));
		}
	}
}
