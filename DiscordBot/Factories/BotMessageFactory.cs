using Discord;

namespace Discord_Channel_Importer.DiscordBot.Factories
{
	internal static class BotMessageFactory
	{
		public static Embed CreateEmbed(BotMessageType msgType)
		{
			switch (msgType)
			{
				case BotMessageType.InvalidChannel:
					return DiscordFactory.CreateEmbed("Invalid channel!", "The channel you passed doesn't exist. (Example: #my-channel)", Color.Red);

				case BotMessageType.NoImports:
					return DiscordFactory.CreateEmbed("Not Imports!", "I am not importing anything at the moment.", Color.Red);

				case BotMessageType.NoImportForChannel:
					return DiscordFactory.CreateEmbed("Not Importing!", "I am not importing to that channel.", Color.Red);
			}

			return null;
		}
	}
}
