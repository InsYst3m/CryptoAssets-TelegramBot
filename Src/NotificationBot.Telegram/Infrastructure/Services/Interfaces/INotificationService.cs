using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService : IDiagnosticService
    {
        /// <summary>
        /// Sends the notification to the telegram user asynchronous.
        /// </summary>
        /// <param name="chatId">The chat identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> SendNotificationAsync(
            long chatId,
            string message,
            CancellationToken cancellationToken = default);

        Task<bool> SendMarkupNotificationAsync(
            long chatId,
            string message,
            IReplyMarkup replyMarkup,
            CancellationToken cancellationToken = default);

        Task<bool> IsValidTimeIntervalAsync(long userId);
    }
}
