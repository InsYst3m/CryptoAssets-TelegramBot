using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public class GetAssetsCommand : Command
	{
		public GetAssetsCommand(long receiverId) 
			: base(ConstantsHelper.Commands.GET_ASSETS, receiverId)
		{
		}
	}
}
