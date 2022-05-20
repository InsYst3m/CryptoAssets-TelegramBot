using Microsoft.Extensions.Caching.Memory;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public class BotCommandFactory : IBotCommandFactory
    {
        private const string PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY = "PeriodicNotificationCommandCache";

        private readonly IDataAccessService _dataAccessService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;

        public BotCommandFactory(
            IDataAccessService dataAccessService,
            IServiceProvider serviceProvider, 
            IMemoryCache memoryCache)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(memoryCache);

            _dataAccessService = dataAccessService;
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
        }

        public async Task<IBotCommand?> GetOrCreateAsync(ParsedMessage parsedMessage)
        {
            List<string> supportedCryptoAssetsAbbreviations = 
                (await _dataAccessService.GetCryptoAssetsLookupAsync())
                .Select(x => x.Abbreviation)
                .ToList();

            if (string.IsNullOrWhiteSpace(parsedMessage.Command))
            {
                return null;
            }

            IBotCommand? botCommand = parsedMessage.Command switch
            {
                "/favourites" or
                "/favorites" => new FavoriteCryptoAssetsCommand(
                    parsedMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>(),
                    _serviceProvider.GetRequiredService<IGraphService>(),
                    _serviceProvider.GetRequiredService<IMessageGenerator>()),

                "/start" => new BotStartCommand(
                    parsedMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>()),

                "/stop" => new BotStopCommand(
                    parsedMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>()),

                string value when supportedCryptoAssetsAbbreviations.Contains(parsedMessage.CommandText!)
                    => new CryptoAssetInfoCommand(
                        _serviceProvider.GetRequiredService<IDataAccessService>(),
                        _serviceProvider.GetRequiredService<IGraphService>(),
                        _serviceProvider.GetRequiredService<IMessageGenerator>()),

                _ => null
            };

            return botCommand;
        }

        public IBotCommand GetOrCreatePeriodicNotificationCommand()
        {
            return _memoryCache.GetOrCreate(
                PERIODIC_NOTIFICATION_COMMAND_CACHE_KEY,
                cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(360);

                    return new PeriodicNotificationCommand(
                        _serviceProvider.GetRequiredService<IDataAccessService>(),
                        _serviceProvider.GetRequiredService<IGraphService>(),
                        _serviceProvider.GetRequiredService<IMessageGenerator>(),
                        _serviceProvider.GetRequiredService<INotificationService>(),
                        _serviceProvider.GetRequiredService<IBotClientFactory>());
                });
        }
    }
}
