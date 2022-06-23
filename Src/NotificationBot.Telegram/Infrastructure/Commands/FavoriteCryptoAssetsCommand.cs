using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class FavoriteCryptoAssetsCommand : IBotCommand
    {
        private const string FAVORITE_CRYPTO_ASSETS_NOT_FOUND = "You have not selected any favorite crypto asset yet.";

        private readonly ParsedMessage _parsedMessage;
        private readonly IDataAccessService _dataAccessService;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteCryptoAssetsCommand"/> class.
        /// </summary>
        /// <param name="parsedMessage">The parsed telegram bot message.</param>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        public FavoriteCryptoAssetsCommand(
            ParsedMessage parsedMessage,
            IDataAccessService dataAccessService,
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);
            
            _parsedMessage = parsedMessage;
            _dataAccessService = dataAccessService;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;
        }

        /// <inheritdoc cref="IBotCommand.ExecuteAsync(string[])" />
        public async Task ExecuteAsync(params string[] arguments)
        {
            string message = string.Empty;

            string[] cryptoAssetsAbbreviations = 
                (await _dataAccessService.GetFollowedCryptoAssetsByTelegramUserIdAsync(_parsedMessage.Message.Chat.Id))
                .Select(x => x.Abbreviation)
                .ToArray();

            List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

            if (cryptoAssets.Any())
            {
                message = _messageGenerator.GenerateFavoriteCryptoAssetsInfoMessageAsync(cryptoAssets);
            } 
            else
            {
                message = "You have not selected any favorite crypto asset yet.";
            }

            await _notificationService.SendNotificationAsync(_parsedMessage.Message.Chat.Id, message);
        }
    }
}
