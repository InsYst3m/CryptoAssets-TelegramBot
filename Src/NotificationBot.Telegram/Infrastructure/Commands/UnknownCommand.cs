using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public class UnknownCommand : Command
	{
		public UnknownCommand(long receiverId) : base(ConstantsHelper.Commands.UNKNOWN, receiverId)
		{
		}
	}
}
