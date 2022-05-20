using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class FavouriteCryptoAssetsCommand : IBotCommand
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavouriteCryptoAssetsCommand"/> class.
        /// </summary>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        public FavouriteCryptoAssetsCommand(
            IDataAccessService dataAccessService,
            IGraphService graphService,
            IMessageGenerator messageGenerator)
        {
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);

            _dataAccessService = dataAccessService;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
        }

        /// <inheritdoc cref="IBotCommand.ExecuteAsync(string[])" />
        public async Task<string> ExecuteAsync(params string[] arguments)
        {
            // TODO: get userId from ParsedMessage

            string[] cryptoAssetsAbbreviations = 
                (await _dataAccessService.GetFavouriteCryptoAssetsAsync(1))
                .Select(x => x.Abbreviation)
                .ToArray();

            List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

            if (!cryptoAssets.Any())
            {
                return "You have not selected any favourite crypto asset yet.";
            }

            return _messageGenerator.GenerateFavouriteCryptoAssetsInfoMessageAsync(cryptoAssets);
        }
    }
}
