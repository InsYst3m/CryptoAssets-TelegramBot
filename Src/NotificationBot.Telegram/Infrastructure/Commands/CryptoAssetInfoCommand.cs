using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;
using NotificationBot.Telegram.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class CryptoAssetInfoCommand : IBotCommand
    {
        private const string CRYPTO_ASSET_NOT_SUPPORTED = "Crypto Asset is not supported.";
        private const string CRYPTO_ASSET_NOT_FOUND = "Crypto Asset not found.";

        private readonly CommandMessage _commandMessage;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoAssetInfoCommand"/> class.
        /// </summary>
        /// <param name="commandMessage">The parsed telegram bot message.</param>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        /// <param name="notificationService">The notification service.</param>
        public CryptoAssetInfoCommand(
            CommandMessage commandMessage,
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(commandMessage);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);

            _commandMessage = commandMessage;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;
        }

        /// <inheritdoc cref="IBotCommand.ExecuteAsync(string[])" />
        public async Task ExecuteAsync()
        {
            CryptoAssetViewModel? cryptoAsset = await _graphService.GetCryptoAssetAsync(_commandMessage.Arguments[0]);

            if (cryptoAsset is null)
            {
                await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, CRYPTO_ASSET_NOT_FOUND);
                return;
            }

            string result = _messageGenerator.GenerateCryptoAssetInfoMessageAsync(cryptoAsset);

            await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, result);
        }
    }
}
