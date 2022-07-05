using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;
using NotificationBot.Telegram.Models;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class CryptoAssetInfoCommand : IBotCommand
    {
        private const string CRYPTO_ASSET_NOT_SUPPORTED = "Crypto Asset is not supported.";
        private const string CRYPTO_ASSET_NOT_FOUND = "Crypto Asset not found.";

        private readonly CommandMessage _commandMessage;
        private readonly IDataAccessService _dataAccessService;
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
            IDataAccessService dataAccessService,
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(commandMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);

            _commandMessage = commandMessage;
            _dataAccessService = dataAccessService;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;
        }

        /// <inheritdoc cref="IBotCommand.ExecuteAsync(string[])" />
        public async Task ExecuteAsync()
        {
            List<CryptoAsset> supportedCryptoAssets = await _dataAccessService.GetCryptoAssetsLookupAsync();

            if (!supportedCryptoAssets.Exists(x => x.Abbreviation == _commandMessage.CommandText!))
{
                await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, CRYPTO_ASSET_NOT_SUPPORTED);
                return;
            }

            CryptoAssetViewModel? cryptoAsset = await _graphService.GetCryptoAssetAsync(_commandMessage.CommandText!);

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
