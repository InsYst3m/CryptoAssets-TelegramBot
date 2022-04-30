using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public interface IBotHandlers
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}