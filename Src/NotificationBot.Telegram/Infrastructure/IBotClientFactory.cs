using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure
{
    public interface IBotClientFactory
    {
        TelegramBotClient GetOrCreate(string token);
    }
}