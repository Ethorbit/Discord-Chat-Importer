namespace Discord_Channel_Importer.DiscordBot.Commands
{
	public struct CommandInfo
	{
		public Modules.CommandModule Command { get; set; }
		public string Usage { get; set; }
		public string Description { get; set; }
	}
}
