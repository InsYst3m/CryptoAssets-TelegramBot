using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
	public class GetAssetCommandProcessor : IBotCommandProcessor
	{
		#region Constants

		private const string CRYPTO_ASSET_NOT_FOUND = "Crypto Asset not found.";

		#endregion

		private readonly GetAssetCommand _command;

		private readonly IGraphService _graphService;
		private readonly IMessageGenerator _messageGenerator;
		private readonly INotificationService _notificationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetAssetCommandProcessor"/> class.
		/// </summary>
		/// <param name="command">The parsed telegram bot message.</param>
		/// <param name="graphService">The graph service.</param>
		/// <param name="messageGenerator">The message generator.</param>
		/// <param name="notificationService">The notification service.</param>
		public GetAssetCommandProcessor(
			GetAssetCommand command,
			IGraphService graphService,
			IMessageGenerator messageGenerator,
			INotificationService notificationService)
		{
			_command = command ?? throw new ArgumentNullException(nameof(command));
			_graphService = graphService ?? throw new ArgumentNullException(nameof(graphService));
			_messageGenerator = messageGenerator ?? throw new ArgumentNullException(nameof(messageGenerator));
			_notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
		}

		/// <inheritdoc cref="IBotCommandProcessor.ExecuteAsync(string[])" />
		public async Task ProcessAsync()
		{
			CryptoAssetViewModel? cryptoAsset = await _graphService.GetCryptoAssetAsync(_command.Asset);

			if (cryptoAsset is null)
			{
				await _notificationService.SendNotificationAsync(_command.ReceiverId, CRYPTO_ASSET_NOT_FOUND);
				return;
			}

			string result = _messageGenerator.GenerateCryptoAssetInfoMessageAsync(cryptoAsset);

			await _notificationService.SendNotificationAsync(_command.ReceiverId, result);
		}
	}
}
