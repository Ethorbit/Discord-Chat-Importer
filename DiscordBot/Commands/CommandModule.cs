using Discord;
using Discord.Commands;
using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.Commands.Modules
{
	/// <summary>
	/// A bot command.
	/// </summary>
	public abstract class CommandModule : ModuleBase<BotSocketCommandContext>
	{
		public abstract CommandInfo[] CommandInfo { get; }
		//public abstract string Usage { get; }
		//public abstract string Description { get; }

		protected const GuildPermission DefaultPermission = (GuildPermission.ManageMessages | GuildPermission.ManageChannels);

		public CommandModule()
		{
			if (this.CommandInfo != null)
				Commands.Add(this.CommandInfo);

				//Commands.Add(new CommandInfo() { Command = this, Usage = this.Usage, Description = this.Description });
		}
	}
}
