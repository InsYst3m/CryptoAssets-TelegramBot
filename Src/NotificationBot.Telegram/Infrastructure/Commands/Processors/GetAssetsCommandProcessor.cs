using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
	public class GetAssetsCommandProcessor : IBotCommandProcessor
	{
		private readonly GetAssetsCommand _command;
		private readonly IGraphService _graphService;
		private readonly INotificationService _notificationService;

		public GetAssetsCommandProcessor(
			GetAssetsCommand command,
			IGraphService graphService,
			INotificationService notificationService)
		{
			_command = command ?? throw new ArgumentNullException(nameof(command));
			_graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));
			_notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
		}

		public async Task ProcessAsync()
		{
			string[] cryptoAssets = await _graphService.GetSupportedCryptoAssetsAsync();

			string result = "Supported crypto assets:\n" + string.Join("\n", cryptoAssets);

			await _notificationService.SendNotificationAsync(_command.ReceiverId, result);
		}
	}
}
