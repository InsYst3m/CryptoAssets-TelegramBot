using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.HostedServices
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly BotSettings _botSettings;
        private readonly TelegramBotClient _botClient;
        private readonly IBotService _botService;

        public TelegramBotHostedService(
            IOptions<BotSettings> botSettings,
            ITelegramBotClientFactory botClientFactory,
            IBotService botService)
        {
            if (botSettings is null || string.IsNullOrWhiteSpace(botSettings.Value.Token))
            {
                throw new ArgumentNullException(nameof(botSettings));
            }

            _ = botClientFactory ?? throw new ArgumentNullException(nameof(botClientFactory));

            _botSettings = botSettings.Value;
            _botService = botService ?? throw new ArgumentNullException(nameof(botService));
            _botClient = botClientFactory.GetOrCreate(_botSettings.Token);
        }

        #region IHostedService Implementation

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Successfully started. Token: {_botSettings.Token}.");

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = { }
            };

            _botClient.StartReceiving(
                _botService.HandleUpdateAsync,
                _botService.HandleErrorAsync,
                receiverOptions,
                cancellationToken);

            // TODO: one time a day timer
            await _botService.SendNotificationAsync(_botClient, cancellationToken);

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion


    }
}
