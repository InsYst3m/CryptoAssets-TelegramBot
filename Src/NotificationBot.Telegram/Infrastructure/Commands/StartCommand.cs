using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public class StartCommand : Command
	{
		public StartCommand(long receiverId) 
			: base(ConstantsHelper.Commands.START, receiverId)
		{
		}
	}
}
