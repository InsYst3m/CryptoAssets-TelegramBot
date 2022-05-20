using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService : IDiagnosticService
    {
        /// <summary>
        /// Sends the notification to the telegram user asynchronous.
        /// </summary>
        /// <param name="botClient">The bot client.</param>
        /// <param name="chatId">The chat identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> SendNotificationAsync(
            ITelegramBotClient botClient,
            long chatId,
            string message,
            CancellationToken cancellationToken = default);

        Task<bool> IsValidTimeIntervalAsync(long userId);
    }
}
