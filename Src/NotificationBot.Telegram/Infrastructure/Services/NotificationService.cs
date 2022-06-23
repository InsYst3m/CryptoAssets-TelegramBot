using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotifiicationBot.Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationsSettings _notificationsSettings;
        private readonly IDataAccessService _dataAccessService;
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notificationsSettings">The notifications settings.</param>
        /// <param name="dataAccessService">The data access service.</param>
        public NotificationService(
            IOptions<NotificationsSettings> notificationsSettings,
            IDataAccessService dataAccessService,
            IBotClientFactory botClientFactory)
        {
            ArgumentNullException.ThrowIfNull(notificationsSettings);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(botClientFactory);

            _notificationsSettings = notificationsSettings.Value;
            _dataAccessService = dataAccessService;
            _botClient = botClientFactory.GetOrCreate();
        }

        /// <inheritdoc cref="INotificationService.SendNotificationAsync(ITelegramBotClient, long, string, CancellationToken)" />
        public async Task<bool> SendNotificationAsync(
            long chatId,
            string message,
            CancellationToken cancellationToken = default)
        {
            if (!await IsValidTimeIntervalAsync(chatId))
            {
                return false;
            }

            await _botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);

            return true;
        }

        public async Task<bool> SendMarkupNotificationAsync(
            long chatId,
            string message,
            IReplyMarkup replyMarkup,
            CancellationToken cancellationToken = default)
        {
            await _botClient.SendTextMessageAsync(
                chatId,
                message,
                replyMarkup: replyMarkup,
                cancellationToken: cancellationToken);

            return true;
        }

        /// <summary>
        /// Determines whether is valid time interval to send notifications to user asynchronous.
        /// </summary>
        public async Task<bool> IsValidTimeIntervalAsync(long userId)
        {
            UserSettings? userSettings = await _dataAccessService.GetUserSettingsAsync(userId);

            if (userSettings == null || !userSettings.UseSleepHours)
            {
                return true;
            }

            DateTime utcNow = DateTime.UtcNow;

            DateTime sleepTimeStartUtc = DateTime
                .SpecifyKind(DateTime.Today, DateTimeKind.Utc)
                .Subtract(userSettings.SleepTimeStart!.Value);

            DateTime sleepTimeEndUtc = DateTime
                .SpecifyKind(DateTime.Today, DateTimeKind.Utc)
                .Subtract(userSettings.SleepTimeEnd!.Value);

            if (utcNow > sleepTimeStartUtc && utcNow < sleepTimeEndUtc)
            {
                return false;
            }

            return true;
        }

        #region IDiagnosticService Implementation

        public Dictionary<string, string> GetDiagnosticsInfo()
        {
            return new Dictionary<string, string>
            { };
        }

        #endregion
    }
}
