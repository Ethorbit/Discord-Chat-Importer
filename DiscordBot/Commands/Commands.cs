using Discord;
using System.Collections.Generic;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public static class Commands
	{
		private static readonly List<CommandInfo[]> CommandsList = new List<CommandInfo[]>();

		public static void Add(CommandInfo[] info)
		{
			CommandsList.Add(info);
		}

		public static List<CommandInfo[]> GetAll()
		{
			return CommandsList;
		}
	}
}
