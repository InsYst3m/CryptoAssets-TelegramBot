using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Infrastructure.Generators;
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
        private readonly IMessageGenerator _messageGenerator;
        private readonly IDataAccessService _dataAccessService;

        public TelegramBotHostedService(
            IOptions<BotSettings> botSettings,
            ITelegramBotClientFactory botClientFactory,
            IBotService botService,
            IMessageGenerator messageGenerator,
            IDataAccessService dataAccessService)
        {
            if (botSettings is null || string.IsNullOrWhiteSpace(botSettings.Value.Token))
            {
                throw new ArgumentNullException(nameof(botSettings));
            }

            _ = botClientFactory ?? throw new ArgumentNullException(nameof(botClientFactory));

            _botSettings = botSettings.Value;
            _botService = botService ?? throw new ArgumentNullException(nameof(botService));
            _messageGenerator = messageGenerator ?? throw new ArgumentNullException(nameof(messageGenerator));
            _dataAccessService = dataAccessService ?? throw new ArgumentNullException(nameof(dataAccessService));
            _botClient = botClientFactory.GetOrCreate(_botSettings.Token);
            _tokenSource = new CancellationTokenSource();
        }

        #region IHostedService Implementation

        public async Task StartAsync(CancellationToken cancellationToken)
        {
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
            PeriodicTimer periodicTimer = new(TimeSpan.FromHours(3));

            while (await periodicTimer.WaitForNextTickAsync())
            {
                if (IsValidTimeInterval())
                {
                    List<CryptoAsset> cryptoAssets = await _dataAccessService.GetFavoriteCryptoAssets(1);
                    string message = await _messageGenerator.GenerateCryptoAssetsMessageAsync(cryptoAssets);
                    await _botService.SendNotificationAsync(_botClient, _botSettings.ChatId!, message, _tokenSource.Token);
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
