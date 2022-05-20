using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure
{
    public class BotClientFactory : IBotClientFactory
    {
        private const string TELEGRAM_BOT_CLIENT_CACHE_KEY = "TelegramBotClientCache";

        private readonly BotSettings _botSettings;
        private readonly IMemoryCache _memoryCache;

        public BotClientFactory(
            IOptions<BotSettings> botSettings,
            IMemoryCache memoryCache)
{
            ArgumentNullException.ThrowIfNull(botSettings);
            ArgumentNullException.ThrowIfNull(botSettings.Value.Token, nameof(botSettings));
            ArgumentNullException.ThrowIfNull(memoryCache);

            _botSettings = botSettings.Value;
            _memoryCache = memoryCache;
        }

        public TelegramBotClient GetOrCreate()
        {
            return _memoryCache.GetOrCreate(
                TELEGRAM_BOT_CLIENT_CACHE_KEY,
                cacheEntry =>
                {
                    cacheEntry.Priority = CacheItemPriority.NeverRemove;

                    return new TelegramBotClient(_botSettings.Token!);
                });
        }
    }
}
