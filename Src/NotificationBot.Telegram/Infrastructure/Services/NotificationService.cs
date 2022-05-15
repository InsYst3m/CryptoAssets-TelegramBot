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
            long chatId,
            string message,
            CancellationToken cancellationToken)
        {
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
    }
}
