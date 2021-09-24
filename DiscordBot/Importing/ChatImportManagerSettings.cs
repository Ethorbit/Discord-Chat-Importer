namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ChatImportManagerSettings : IChatImportManagerSettings
	{
		public double ImportTime { get; }
		public double AddedTimeForEachImporter { get; }
		public int MaxSimultaneousImports { get; }

		/// <param name="importTime">The time it takes for a ChatImporter to import a message to a channel.</param>
		/// <param name="addedTimeForEachImporter">The extra Import time for each additional Importer</param>
		/// <param name="maxSimultaneousImports">The maximum allowed importers active at any time</param>
		public ChatImportManagerSettings(int importTime, int addedTimeForEachImporter, int maxSimultaneousImports)
		{
			this.ImportTime = importTime;
			this.AddedTimeForEachImporter = addedTimeForEachImporter;
			this.MaxSimultaneousImports = maxSimultaneousImports;
		}
	}
}
