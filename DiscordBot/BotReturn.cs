namespace Discord_Channel_Importer.DiscordBot
{
	/// <summary>
	/// Possible return values from Bot operations
	/// </summary>
	internal enum BotReturn
	{
		Success,
		Error,
		ParseError,
		ImporterExists,
		ImporterDoesntExist,
		MaxImportsReached
	}
}
