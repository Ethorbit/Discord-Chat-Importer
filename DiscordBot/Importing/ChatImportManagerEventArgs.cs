namespace Discord_Channel_Importer.DiscordBot.Importing
{
	internal class ChatImportManagerEventArgs
	{
		public IChatImporter Importer { get; }
		public IChatImporterSettings ImporterSettings { get; }

		public ChatImportManagerEventArgs(IChatImporter importer, IChatImporterSettings importerSettings)
		{
			this.Importer = importer;
			this.ImporterSettings = importerSettings;
		}
	}
}
