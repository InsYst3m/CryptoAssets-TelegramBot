using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public class BotCommandFactory : IBotCommandFactory
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IServiceProvider _serviceProvider;

        public BotCommandFactory(IDataAccessService dataAccessService, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(serviceProvider);

            _dataAccessService = dataAccessService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IBotCommand?> GetOrCreateAsync(string command)
        {
            // TODO: add cache
            List<string> supportedCryptoAssetsAbbreviations = 
                (await _dataAccessService.GetCryptoAssetsLookupAsync())
                .Select(x => x.Abbreviation)
                .ToList();

            IBotCommand? botCommand = command switch
            {
                string text when text.StartsWith('/') => text switch 
                {
                    string textCommand when supportedCryptoAssetsAbbreviations.Contains(textCommand[1..])
                        => new CryptoAssetInfoCommand(
                                _serviceProvider.GetRequiredService<IDataAccessService>(),
                                _serviceProvider.GetRequiredService<IGraphService>(),
                                _serviceProvider.GetRequiredService<IMessageGenerator>()),

                    "/favourites" or
                    "/favorites"
                        => new FavouriteCryptoAssetsCommand(
                                _serviceProvider.GetRequiredService<IDataAccessService>(),
                                _serviceProvider.GetRequiredService<IGraphService>(),
                                _serviceProvider.GetRequiredService<IMessageGenerator>()),

                    _ => null
                },
                _ => null
            };

            return botCommand;
        }
    }
}
