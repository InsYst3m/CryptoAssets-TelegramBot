using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
    public class GetFavoriteAssetsCommandProcessor : IBotCommandProcessor
    {
        private readonly GetFavoriteAssetsCommand _command;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;
        private readonly INotificationService _notificationService;

        public GetFavoriteAssetsCommandProcessor(
			GetFavoriteAssetsCommand command,
            IGraphService graphService,
            IMessageGenerator messageGenerator,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(notificationService);

            _command = command;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
            _notificationService = notificationService;
        }

        /// <inheritdoc cref="IBotCommandProcessor.ExecuteAsync(string[])" />
        public async Task ProcessAsync()
        {
             //_graphService.GetUserFavoriteAssets(receiverId);

            //string[] cryptoAssetsAbbreviations = 
            //    (await _dataAccessService.GetFollowedCryptoAssetsByTelegramUserIdAsync(_commandMessage.Message.Chat.Id))
            //    .Select(x => x.Abbreviation)
            //    .ToArray();

            //List<CryptoAssetViewModel> cryptoAssets = await _graphService.GetCryptoAssetsAsync(cryptoAssetsAbbreviations);

            //if (cryptoAssets.Any())
            //{
            //    message = _messageGenerator.GenerateFavoriteCryptoAssetsInfoMessageAsync(cryptoAssets);
            //} 
            //else
            //{
            //    message = "You have not selected any favorite crypto asset yet.";
            //}

            await _notificationService.SendNotificationAsync(_command.ReceiverId, "Get Favorite Assets Command");
        }
    }
}
