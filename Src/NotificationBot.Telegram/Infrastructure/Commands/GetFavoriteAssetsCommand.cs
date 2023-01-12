using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public class GetFavoriteAssetsCommand : Command
	{
		public GetFavoriteAssetsCommand(long receiverId) 
			: base(ConstantsHelper.Commands.GET_ASSETS, receiverId)
		{
		}
	}
}
