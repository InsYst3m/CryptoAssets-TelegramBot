using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class CryptoAssetInfoCommand : IBotCommand
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoAssetInfoCommand"/> class.
        /// </summary>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        public CryptoAssetInfoCommand(
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
            // TODO: refactor to ParsedMessage

            if (arguments.Length == 0)
            {
                return "Text command not found.";
            }

            string cryptoAssetAbbreviation = await GetCryptoAssetAbbreviationFromCommandAsync(arguments[0]);

            CryptoAssetViewModel? cryptoAsset = await _graphService.GetCryptoAssetAsync(cryptoAssetAbbreviation);

            if (cryptoAsset is null)
            {
                return "Crypto Asset not found.";
            }

            return _messageGenerator.GenerateCryptoAssetInfoMessageAsync(cryptoAsset);
        }

        #region Internal Implementation

        /// <summary>
        /// Gets the crypto asset abbreviation from telegram bot text command.
        /// </summary>
        /// <param name="command">The telegram bot text command.</param>
        /// <returns>Returns crypto asset abbreviation.</returns>
        private async Task<string> GetCryptoAssetAbbreviationFromCommandAsync(string command)
        {
            // TODO: temp solution

            if (!command.StartsWith('/'))
            {
                throw new ArgumentException("Provided text is not a command.", nameof(command));
            }

            string abbreviation = command[1..];

            List<CryptoAsset> supportedCryptoAssets = await _dataAccessService.GetCryptoAssetsLookupAsync();

            if (!supportedCryptoAssets.Exists(x => x.Abbreviation == abbreviation))
            {
                throw new ArgumentException("Provided crypto asset is not supported.");
            }

            return abbreviation;
        }

        #endregion
    }
}
