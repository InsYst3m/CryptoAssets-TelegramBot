using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Services;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace NotificationBot.Telegram.Infrastructure.HostedServices
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly CancellationTokenSource _tokenSource;

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
            _tokenSource = new CancellationTokenSource();
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

            await SchedulePeriodicTimer();

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            return Task.CompletedTask;
        }

        #endregion

        #region Periodic Timer Logic

        private async Task SchedulePeriodicTimer()
        {
            PeriodicTimer periodicTimer = new(TimeSpan.FromSeconds(10));

            while (await periodicTimer.WaitForNextTickAsync())
            {
                if (IsValidTimeInterval())
                {
                    await _botService.SendNotificationAsync(_botClient, _botSettings.ChatId!, _tokenSource.Token);
                }
            }
        }

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
    }
}
