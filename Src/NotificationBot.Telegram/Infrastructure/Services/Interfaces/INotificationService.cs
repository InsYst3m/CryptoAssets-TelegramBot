using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService : IDiagnosticService
    {
        Task<bool> SendNotificationAsync(ITelegramBotClient botClient, long? chatId, string message, CancellationToken cancellationToken);

        Task<bool> IsValidTimeIntervalAsync(long userId);
    }
}
