namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Possible return values from Bot operations
	/// </summary>
	public enum BotReturn
	{
		Success,
		Error,
		ParseError,
		ImporterExists,
		ImporterDoesntExist,
		MaxImportsReached
	}
}
