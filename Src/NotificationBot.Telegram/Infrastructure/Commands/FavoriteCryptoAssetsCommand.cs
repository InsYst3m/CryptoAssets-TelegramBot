using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class FavoriteCryptoAssetsCommand : IBotCommand
    {
        private const string FAVORITE_CRYPTO_ASSETS_NOT_FOUND = "You have not selected any favorite crypto asset yet.";

        private readonly CommandMessage _commandMessage;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteCryptoAssetsCommand"/> class.
        /// </summary>
        /// <param name="commandMessage">The parsed telegram bot message.</param>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        public FavoriteCryptoAssetsCommand(
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
            string message = string.Empty;

            //string[] cryptoAssetsAbbreviations = 
            //    (await _dataAccessService.GetFollowedCryptoAssetsByTelegramUserIdAsync(_commandMessage.Message.Chat.Id))
            //    .Select(x => x.Abbreviation)
            //    .ToArray();

            //List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

            //if (cryptoAssets.Any())
            //{
            //    message = _messageGenerator.GenerateFavoriteCryptoAssetsInfoMessageAsync(cryptoAssets);
            //} 
            //else
            //{
            //    message = "You have not selected any favorite crypto asset yet.";
            //}

            await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, message);
        }
    }
}
