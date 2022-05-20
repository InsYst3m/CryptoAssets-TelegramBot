using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public class BotCommandFactory : IBotCommandFactory
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IServiceProvider _serviceProvider;

        public BotCommandFactory(
            IDataAccessService dataAccessService,
            IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(serviceProvider);

            _dataAccessService = dataAccessService;
            _serviceProvider = serviceProvider;
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
    }
}
