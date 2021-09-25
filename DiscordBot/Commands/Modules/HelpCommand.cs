using Discord;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public class HelpCommand : CommandModule
	{
		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Importer Commands", null, Color.LightGrey,
				new EmbedField[] {
					DiscordFactory.CreateEmbedField("import <url> #channel-name", @"Gets all Discord messages from the 
																	provided URL (which should be a .json 
																	with the proper Discord channel structure) 
																	and recreates them in the provided channel."),
					DiscordFactory.CreateEmbedField("cancel #channel-name", @"Makes me stop importing to a channel that I am currently sending
																			archived messages to."),
					DiscordFactory.CreateEmbedField("undo #channel-name", "Removes ALL archived messages I made in the specified channel and automatically cancels importing to that channel.")
				}
			));
		}
	}
}
