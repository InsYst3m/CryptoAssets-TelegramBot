using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.GraphService;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;
using StrawberryShake;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class BotService : IBotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly BotSettings _botSettings;
        private readonly NotificationsSettings _notificationsSettings;

        private readonly IMessageGenerator _messageGenerator;
        private readonly IDataAccessService _dataAccessService;
        private readonly INotificationService _notificationService;

        private readonly ICryptoAssetsGraphServiceClient _graphService;

        public BotService(
            IOptions<BotSettings> botSettings,
            IOptions<NotificationsSettings> notificationsSettings,
            ITelegramBotClientFactory botClientFactory,
            IMessageGenerator messageGenerator,
            IDataAccessService dataAccessService,
            INotificationService notificationService,
            ICryptoAssetsGraphServiceClient graphService)
        {
            ArgumentNullException.ThrowIfNull(botSettings);
            ArgumentNullException.ThrowIfNull(notificationsSettings);
            ArgumentNullException.ThrowIfNull(botSettings.Value.Token, nameof(botSettings));
            ArgumentNullException.ThrowIfNull(botClientFactory);
            ArgumentNullException.ThrowIfNull(messageGenerator);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(notificationService);
            ArgumentNullException.ThrowIfNull(graphService);

            _messageGenerator = messageGenerator;
            _dataAccessService = dataAccessService;
            _notificationService = notificationService;
            _graphService = graphService;

            _botSettings = botSettings.Value;
            _notificationsSettings = notificationsSettings.Value;
            _botClient = botClientFactory.GetOrCreate(_botSettings.Token);
        }

        public void Start(CancellationToken cancellationToken)
        {
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = { }
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);
        }

        public async Task SetupPeriodicNotifications(CancellationToken cancellationToken)
        {
            PeriodicTimer periodicTimer = new(TimeSpan.FromMinutes(_notificationsSettings.IntervalInMinutes));

            while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                if (IsValidTimeInterval())
                {
                    List<CryptoAsset> cryptoAssets = await _dataAccessService.GetFavoriteCryptoAssets(1);

                    StringBuilder sb = new("Favorite Crypto Assets Status:" + Environment.NewLine);

                    foreach (CryptoAsset cryptoAsset in cryptoAssets)
                    {
                        CryptoAssetViewModel? viewModel = await GetCryptoAssetAsync(cryptoAsset.Abbreviation, cancellationToken);

                        if (viewModel != null)
                        {
                            string message = await _messageGenerator.GenerateCryptoAssetsMessageAsync(viewModel);
                            sb.AppendLine(message);
                        }
                    }

                    await _notificationService.SendNotificationAsync(_botClient, _botSettings.ChatId!, sb.ToString(), cancellationToken);
                }
            }
        }

        public async Task<CryptoAssetViewModel?> GetCryptoAssetAsync(string abbreviation, CancellationToken cancellationToken)
        {
            IOperationResult<IGetCryptoAssetResult> cryptoAssetGraphData =
                await _graphService.GetCryptoAsset.ExecuteAsync(abbreviation, cancellationToken);
            cryptoAssetGraphData.EnsureNoErrors();

            return MapToCryptoAssetViewModel(cryptoAssetGraphData.Data?.CryptoAsset);
        }

        #region Internal Implementation

        private CryptoAssetViewModel? MapToCryptoAssetViewModel(IGetCryptoAsset_CryptoAsset? cryptoAssetGraphData)
        {
            if (cryptoAssetGraphData == null)
            {
                return null;
            }

            return new CryptoAssetViewModel(cryptoAssetGraphData.Abbreviation, cryptoAssetGraphData.MarketData!.CurrentPrice!.Usd);
        }

        private bool IsValidTimeInterval()
        {
            DateTime utcNow = DateTime.UtcNow;

            DateTime startDateUtc = DateTime.Today.AddHours(_notificationsSettings.StartHourUTC);
            DateTime endDateUtc = DateTime.Today.AddHours(_notificationsSettings.EndHourUTC);

            if (utcNow > startDateUtc && utcNow < endDateUtc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IBotHandlers Implementation

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }

            var chatId = update.Message.Chat.Id;
            var message = update.Message.Text;

            // Echo message
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId,
                text: $"You said:\n{message}. ChatId: {chatId}.",
                cancellationToken: cancellationToken);
        }

        #endregion
    }
}
