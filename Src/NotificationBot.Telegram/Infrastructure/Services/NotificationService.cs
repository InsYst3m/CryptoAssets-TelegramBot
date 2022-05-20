using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationsSettings _notificationsSettings;
        private readonly IDataAccessService _dataAccessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notificationsSettings">The notifications settings.</param>
        /// <param name="dataAccessService">The data access service.</param>
        public NotificationService(
            IOptions<NotificationsSettings> notificationsSettings,
            IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(notificationsSettings);
            ArgumentNullException.ThrowIfNull(dataAccessService);

            _notificationsSettings = notificationsSettings.Value;
            _dataAccessService = dataAccessService;
        }

        /// <summary>
        /// Sends the notification to the user asynchronous.
        /// </summary>
        /// <param name="botClient">The bot client.</param>
        /// <param name="chatId">The chat identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<bool> SendNotificationAsync(
            ITelegramBotClient botClient,
            long? chatId,
            string message,
            CancellationToken cancellationToken)
        {
            if (chatId == null)
            {
                chatId = _notificationsSettings.ChatId;
            }

            if (!await IsValidTimeIntervalAsync(1))
            {
                return false;
            }

            await botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);

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
                .Subtract(userSettings.SleepTimeStart);

            DateTime sleepTimeEndUtc = DateTime
                .SpecifyKind(DateTime.Today, DateTimeKind.Utc)
                .Subtract(userSettings.SleepTimeEnd);

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
