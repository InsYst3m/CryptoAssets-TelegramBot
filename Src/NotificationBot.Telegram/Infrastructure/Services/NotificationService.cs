using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotifiicationBot.Domain.Entities;
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

        /// <inheritdoc cref="INotificationService.SendNotificationAsync(ITelegramBotClient, long, string, CancellationToken)" />
        public async Task<bool> SendNotificationAsync(
            ITelegramBotClient botClient,
            long chatId,
            string message,
            CancellationToken cancellationToken = default)
        {
            if (!await IsValidTimeIntervalAsync(chatId))
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
