using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Handlers;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class BotService : IBotService
    {
        private readonly TelegramBotClient _botClient;

        private readonly IBotHandler _botHandler;
        private readonly ITimerWrapper _timerProvider;

        public BotService(
            IBotClientFactory botClientFactory,
            IBotHandler botHandler,
            ITimerWrapper timerProvider)
        {
            ArgumentNullException.ThrowIfNull(botClientFactory);
            ArgumentNullException.ThrowIfNull(botHandler);
            ArgumentNullException.ThrowIfNull(timerProvider);

            _timerProvider = timerProvider;
            _botHandler = botHandler;

            _botClient = botClientFactory.GetOrCreate();
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

            _timerProvider.OnPeriodicTimerTickEventHandler +=
                (sender, args) => _botHandler.HandlePeriodicTimerTickAsync(
                    _botClient,
                    cancellationToken);

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
