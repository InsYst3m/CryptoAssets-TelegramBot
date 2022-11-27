using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
    public class PeriodicNotificationCommandProcessor : IBotCommandProcessor
    {
        private const string FAVORITE_CRYPTO_ASSETS_NOT_FOUND = "You have not selected any favorite crypto asset yet.";

        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicNotificationCommandProcessor"/> class.
        /// </summary>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="botClientFactory">The telegram bot client factory.</param>
        public PeriodicNotificationCommandProcessor(
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);

            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;
        }

        public async Task ProcessAsync()
        {
            string message = string.Empty;

            //List<User> users = await _dataAccessService.GetUsersToSendPeriodicNotificationsAsync();

            //foreach (User user in users)
            //{
            //    string[] cryptoAssetsAbbreviations =
            //        (await _dataAccessService.GetFollowedCryptoAssetsByTelegramUserIdAsync(user.ChatId))
            //        .Select(x => x.Abbreviation)
            //        .ToArray();

            //    List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

            //    if (cryptoAssets.Any())
            //    {
            //        message = _messageGenerator.GenerateFavoriteCryptoAssetsInfoMessageAsync(cryptoAssets);
            //    }
            //    else
            //    {
            //        message = FAVORITE_CRYPTO_ASSETS_NOT_FOUND;
            //    }

            //    await _notificationService.SendNotificationAsync(user.ChatId, message);
            //}
        }
    }
}
