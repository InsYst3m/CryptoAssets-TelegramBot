using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure
{
    public class BotClientFactory : IBotClientFactory
    {
        public TelegramBotClient GetOrCreate(string token)
        {
            return new TelegramBotClient(token);
        }
    }
}
