using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Discord_Channel_Importer.DiscordBot.Commands
{
	/// <summary>
	/// Workaround for Discord.net API bug where FromError does not output to the Discord channel..
	/// </summary>
	public class RequireUserPermissionWithError : RequireUserPermissionAttribute
	{
		public RequireUserPermissionWithError(Discord.GuildPermission perm) : base(perm) { }

		public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			PreconditionResult res = await base.CheckPermissionsAsync(context, command, services);

			if (res.Error == CommandError.UnmetPrecondition)
			{
				await context.Channel.SendMessageAsync(context.User.Mention + " " + (ErrorMessage ?? $"You need {GuildPermission.Value} permissions to use that command!"));
			}

			return res;
		}
	}
}
