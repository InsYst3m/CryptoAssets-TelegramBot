using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotificationAsync(
            ITelegramBotClient botClient,
            string chatId,
            string message,
            CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);

            await Task.CompletedTask;
        }
    }
}
