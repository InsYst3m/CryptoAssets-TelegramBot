using Microsoft.Extensions.Caching.Memory;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Commands.Portfolio;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;

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

        public async Task<IBotCommand?> GetOrCreateAsync(CommandMessage commandMessage)
        {
            List<string> supportedCryptoAssetsAbbreviations = 
                (await _dataAccessService.GetCryptoAssetsLookupAsync())
                .Select(x => x.Abbreviation)
                .ToList();

            if (string.IsNullOrWhiteSpace(commandMessage.Command))
            {
                return null;
            }

            IBotCommand? botCommand = commandMessage.Command switch
            {
                "/favourites" or
                "/favorites" => new FavoriteCryptoAssetsCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>(),
                    _serviceProvider.GetRequiredService<IGraphService>(),
                    _serviceProvider.GetRequiredService<IMessageGenerator>(),
                    _serviceProvider.GetRequiredService<INotificationService>()),

                "/start" => new BotStartCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>(),
                    _serviceProvider.GetRequiredService<INotificationService>()),

                "/stop" => new BotStopCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>(),
                    _serviceProvider.GetRequiredService<INotificationService>()),

                "/portfolio" => new PortfolioCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<INotificationService>()),

                string value when supportedCryptoAssetsAbbreviations.Contains(commandMessage.CommandText!)
                    => new CryptoAssetInfoCommand(
                        commandMessage,
                        _serviceProvider.GetRequiredService<IDataAccessService>(),
                        _serviceProvider.GetRequiredService<IGraphService>(),
                        _serviceProvider.GetRequiredService<IMessageGenerator>(),
                        _serviceProvider.GetRequiredService<INotificationService>()),

                "/createportfolio" => new PortfolioCreateCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<IDataAccessService>(),
                    _serviceProvider.GetRequiredService<INotificationService>()),

                _ => new NotSupportedCommand(
                    commandMessage,
                    _serviceProvider.GetRequiredService<INotificationService>())
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
                        _serviceProvider.GetRequiredService<INotificationService>());
                });
        }
    }
}
