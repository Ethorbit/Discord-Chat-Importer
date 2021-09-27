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
		public abstract string Usage { get; }
		public abstract string Description { get; }

		protected const GuildPermission DefaultPermission = (GuildPermission.ManageMessages | GuildPermission.ManageChannels);

		public CommandModule()
		{
			if (this.Description != null)
				Commands.Add(new CommandInfo() { Command = this, Usage = this.Usage, Description = this.Description });
		}
	}
}
