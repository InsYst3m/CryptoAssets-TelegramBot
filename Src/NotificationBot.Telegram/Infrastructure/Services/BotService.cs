using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Generators;
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

        private readonly IMessageGenerator _messageGenerator;
        private readonly IDataAccessService _dataAccessService;
        private readonly INotificationService _notificationService;

        public BotService(
            IOptions<BotSettings> botSettings,
            ITelegramBotClientFactory botClientFactory,
            IMessageGenerator messageGenerator,
            IDataAccessService dataAccessService,
            INotificationService notificationService)
        {
            if (botSettings is null || string.IsNullOrWhiteSpace(botSettings.Value.Token))
            {
                throw new ArgumentNullException(nameof(botSettings));
            }

            _messageGenerator = messageGenerator ?? throw new ArgumentNullException(nameof(messageGenerator));
            _dataAccessService = dataAccessService ?? throw new ArgumentNullException(nameof(dataAccessService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            _botSettings = botSettings.Value;
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
            PeriodicTimer periodicTimer = new(TimeSpan.FromHours(3));

            while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                if (IsValidTimeInterval())
                {
                    List<CryptoAsset> cryptoAssets = await _dataAccessService.GetFavoriteCryptoAssets(1);
                    string message = await _messageGenerator.GenerateCryptoAssetsMessageAsync(cryptoAssets);

                    await _notificationService.SendNotificationAsync(_botClient, _botSettings.ChatId!, message, cancellationToken);
                }
            }
        }

        #region Internal Implementation

        private static bool IsValidTimeInterval()
        {
            DateTime utcNow = DateTime.UtcNow;

            DateTime startDateUtc = DateTime.Today.AddHours(6);
            DateTime endDateUtc = DateTime.Today.AddHours(18);

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
