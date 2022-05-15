using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(ITelegramBotClient botClient, long chatId, string message, CancellationToken cancellationToken);

        bool IsValidTimeInterval();
    }
}
