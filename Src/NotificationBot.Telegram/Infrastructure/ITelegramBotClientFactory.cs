using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure
{
    public interface ITelegramBotClientFactory
    {
        TelegramBotClient GetOrCreate(string token);
    }
}