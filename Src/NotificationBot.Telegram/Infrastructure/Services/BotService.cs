using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Handlers;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class BotService : IBotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly BotSettings _botSettings;

        private readonly IBotHandler _botHandler;
        private readonly INotificationService _notificationService;
        private readonly ITimerWrapper _timerProvider;

        public BotService(
            IOptions<BotSettings> botSettings,
            IBotClientFactory botClientFactory,
            IBotHandler botHandler,
            INotificationService notificationService,
            ITimerWrapper timerProvider)
        {
            ArgumentNullException.ThrowIfNull(botSettings);
            ArgumentNullException.ThrowIfNull(botSettings.Value.Token, nameof(botSettings));
            ArgumentNullException.ThrowIfNull(botClientFactory);
            ArgumentNullException.ThrowIfNull(botHandler);
            ArgumentNullException.ThrowIfNull(notificationService);
            ArgumentNullException.ThrowIfNull(timerProvider);

            _notificationService = notificationService;
            _timerProvider = timerProvider;
            _botHandler = botHandler;

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
                _botHandler.HandleUpdateAsync,
                _botHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken);

            _isBotInitialized = true;

            _timerProvider.SetupPeriodicTimer(cancellationToken);
        }

        #region IDiagnosticService Implementation

        private bool _isBotInitialized;

        public Dictionary<string, string> GetDiagnosticsInfo()
        {
            return new Dictionary<string, string>
            {
                { "Bot Initialized", _isBotInitialized.ToString() }
            };
        }

        #endregion
    }
}
