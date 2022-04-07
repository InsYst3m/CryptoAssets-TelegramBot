using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure
{
    public class TelegramBotClientFactory : ITelegramBotClientFactory
    {
        public TelegramBotClient GetOrCreate(string token)
        {
            return new TelegramBotClient(token);
        }
    }
}
