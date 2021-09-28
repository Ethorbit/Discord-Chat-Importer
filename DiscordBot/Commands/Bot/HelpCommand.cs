using Discord;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class HelpCommand : CommandModule
	{
		public override CommandInfo[] CommandInfo { get; }

		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			List<EmbedField> embedFields = new List<EmbedField>();

			foreach (CommandInfo[] infoArr in Commands.GetAll())
			{
				foreach (CommandInfo info in infoArr)
				{
					embedFields.Add(DiscordFactory.CreateEmbedField(info.Usage, info.Description));
				}
			}

			await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Chat Importer Commands", null, Color.LightGrey, embedFields.ToArray()));
		}
	}
}
