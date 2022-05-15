using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationsSettings _notificationsSettings;

        public NotificationService(IOptions<NotificationsSettings> notificationsSettings)
        {
            ArgumentNullException.ThrowIfNull(notificationsSettings);

            _notificationsSettings = notificationsSettings.Value;
        }

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

            if (IsValidTimeInterval())
            {
                await botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);

                return true;
            }

            return false;
        }

        // TODO: Get a valid time interval for sending notifications for each user from DB
        public bool IsValidTimeInterval()
        {
            DateTime utcNow = DateTime.UtcNow;

            DateTime startDateUtc = DateTime
                .SpecifyKind(DateTime.Today, DateTimeKind.Utc)
                .AddHours(_notificationsSettings.StartHourUTC);

            DateTime endDateUtc = DateTime
                .SpecifyKind(DateTime.Today, DateTimeKind.Utc)
                .AddHours(_notificationsSettings.EndHourUTC);

            if (utcNow > startDateUtc && utcNow < endDateUtc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region IDiagnosticService Implementation

        public Dictionary<string, string> GetDiagnosticsInfo()
        {
            TimeSpan startTimeUtc = TimeSpan.FromHours(_notificationsSettings.StartHourUTC);
            TimeSpan endTimeUtc = TimeSpan.FromHours(_notificationsSettings.EndHourUTC);

            TimeSpan startTimeMsk = TimeSpan.FromHours(_notificationsSettings.StartHourUTC + 3);
            TimeSpan endTimeMsk = TimeSpan.FromHours(_notificationsSettings.EndHourUTC + 3);

            return new Dictionary<string, string>
            {
                { "Time period for sending notifications UTC", $"Between {startTimeUtc} and {endTimeUtc}" },
                { "Time period for sending notifications MSK", $"Between {startTimeMsk} and {endTimeMsk}" }
            };
        }

        #endregion
    }
}
