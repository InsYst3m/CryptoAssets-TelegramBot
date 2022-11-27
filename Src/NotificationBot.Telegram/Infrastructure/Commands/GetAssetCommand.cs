using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public class GetAssetCommand : Command
	{
		public readonly string Asset;

		public GetAssetCommand(long receiverId, string asset)
			: base(ConstantsHelper.Commands.GET_ASSET, receiverId)
		{
			if (string.IsNullOrWhiteSpace(asset))
			{
				throw new ArgumentNullException(nameof(asset));
			}

			Asset = asset;
		}
	}
}
