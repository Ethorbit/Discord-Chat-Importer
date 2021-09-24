namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal interface IChatImportManagerSettings
	{
		/// <summary>
		/// The time (in seconds) for an import to be sent to a channel
		/// </summary>
		double ImportTime { get; }

		/// <summary>
		/// The extra time (in seconds) for each importer in-use
		/// </summary>
		double AddedTimeForEachImporter { get; }

		/// <summary>
		/// The maximum Importers allowed at once
		/// </summary>
		int MaxSimultaneousImports { get; }
	}
}
