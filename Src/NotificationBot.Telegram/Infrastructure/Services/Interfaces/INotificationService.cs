using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(ITelegramBotClient botClient, string chatId, string message, CancellationToken cancellationToken);
    }
}
