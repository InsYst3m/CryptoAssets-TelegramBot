using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class PeriodicNotificationCommand : IBotCommand
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicNotificationCommand"/> class.
        /// </summary>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="botClientFactory">The telegram bot client factory.</param>
        public PeriodicNotificationCommand(
            IDataAccessService dataAccessService,
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService,
            IBotClientFactory botClientFactory)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);
            ArgumentNullException.ThrowIfNull(botClientFactory);

            _dataAccessService = dataAccessService;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;

            _botClient = botClientFactory.GetOrCreate();
        }

        public async Task<string> ExecuteAsync(params string[] arguments)
        {
            List<User> users = await _dataAccessService.GetUsersToSendPeriodicNotificationsAsync();

            foreach (User user in users)
            {
                string[] cryptoAssetsAbbreviations =
                    (await _dataAccessService.GetFavoriteCryptoAssetsByTelegramUserIdAsync(user.ChatId))
                    .Select(x => x.Abbreviation)
                    .ToArray();

                List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

                if (!cryptoAssets.Any())
                {
                    return "You have not selected any favorite crypto asset yet.";
                }

                string message = _messageGenerator.GenerateFavoriteCryptoAssetsInfoMessageAsync(cryptoAssets);

                await _notificationService.SendNotificationAsync(_botClient, user.ChatId, message);
            }

            return "Operation completed.";
        }
    }
}
