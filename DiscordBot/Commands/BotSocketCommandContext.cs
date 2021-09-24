using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	/// <summary>
	/// A custom CommandContext that exposes a Context property of the DiscordBot.Bot class 
	/// (Where we started the bot and store all its custom methods and whatnot)
	/// </summary>
	public partial class BotSocketCommandContext 
	{
		//
		// Summary:
		//     Gets the Discord Bot itself
		internal DiscordBot.Bot Bot { get; }

		//
		// Summary:
		//     Initializes a new Discord.Commands.SocketCommandContext class with the provided
		//     client and message.
		//
		// Parameters:
		//   client:
		//     The underlying client.
		//
		//   msg:
		//     The underlying message.
		internal BotSocketCommandContext(DiscordBot.Bot bot, SocketUserMessage msg)
		{
			Bot = bot;
			Client = bot.Settings.Client;
			Guild = (msg.Channel as SocketGuildChannel)?.Guild;
			Channel = msg.Channel;
			User = msg.Author;
			Message = msg;
		}
	}

	public partial class BotSocketCommandContext : ICommandContext // Defaults
	{
		//
		// Summary:
		//     Gets the Discord.WebSocket.DiscordSocketClient that the command is executed with.
		public DiscordSocketClient Client { get; }

		//
		// Summary:
		//     Gets the Discord.WebSocket.SocketGuild that the command is executed in.
		public SocketGuild Guild { get; }

		//
		// Summary:
		//     Gets the Discord.WebSocket.ISocketMessageChannel that the command is executed
		//     in.
		public ISocketMessageChannel Channel { get; }

		//
		// Summary:
		//     Gets the Discord.WebSocket.SocketUser who executed the command.
		public SocketUser User { get; }

		//
		// Summary:
		//     Gets the Discord.WebSocket.SocketUserMessage that the command is interpreted
		//     from.
		public SocketUserMessage Message { get; }

		//
		// Summary:
		//     Indicates whether the channel that the command is executed in is a private channel.
		public bool IsPrivate => Channel is IPrivateChannel;

		IDiscordClient ICommandContext.Client => Client;

		IGuild ICommandContext.Guild => Guild;

		IMessageChannel ICommandContext.Channel => Channel;

		IUser ICommandContext.User => User;

		IUserMessage ICommandContext.Message => Message;
	}
}
