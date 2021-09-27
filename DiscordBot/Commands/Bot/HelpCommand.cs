using Discord;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	public class HelpCommand : CommandModule
	{
		public override string Usage { get; } = null;
		public override string Description { get; } = null;

		[Command("importer", RunMode = RunMode.Async)]
		[Alias("importer help")]
		public async Task ListCommands()
		{
			List<EmbedField> embedFields = new List<EmbedField>();

			foreach (CommandInfo info in Commands.GetAll())
			{
				embedFields.Add(DiscordFactory.CreateEmbedField(info.Usage, info.Description));
			}

			await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Importer Commands", null, Color.LightGrey, embedFields.ToArray()));
		}
	}
}
