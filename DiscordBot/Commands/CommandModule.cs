using Discord;
using Discord.Commands;
using Discord_Channel_Importer.DiscordBot.Factories;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	/// <summary>
	/// A bot command.
	/// </summary>
	public abstract class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		public abstract CommandInfo[] CommandInfo { get; }

		protected const GuildPermission DefaultPermission = (GuildPermission.ManageMessages | GuildPermission.ManageChannels);

		public CommandModule()
		{
			if (this.CommandInfo != null)
				Commands.Add(this.CommandInfo);
		}

		/// <summary>
		/// Outputs the command's usage to the context channel
		/// </summary>
		/// <param name="commandNumber">The command number</param>
		public async Task SendUsageAsync(int commandNumber = 1)
		{
			await ReplyAsync(null, false, DiscordFactory.CreateEmbed("Usage", CommandInfo[commandNumber - 1].Usage, Color.LightGrey));
		}
	}
}
