using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService : IDiagnosticService
    {
        // TODO: temp long?
        Task<bool> SendNotificationAsync(ITelegramBotClient botClient, long? chatId, string message, CancellationToken cancellationToken);

        bool IsValidTimeInterval();
    }
}
