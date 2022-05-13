using NotificationBot.Telegram.Infrastructure.HostedServices.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Services;

namespace NotificationBot.Telegram.Infrastructure.HostedServices
{
    public class TelegramBotHostedService : ITelegramBotHostedService
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly IBotService _botService;

        public TelegramBotHostedService(IBotService botService)
        {
            _botService = botService ?? throw new ArgumentNullException(nameof(botService));
            
            _tokenSource = new CancellationTokenSource();
        }

        #region IHostedService Implementation

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            serviceStartedTimeUTC = DateTime.UtcNow;

            _botService.Start(_tokenSource.Token);
            _ = Task.Run(async () => await _botService.SetupPeriodicNotifications(_tokenSource.Token), _tokenSource.Token);

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            return Task.CompletedTask;
        }

        #endregion

        #region IDiagnosticService Implementation

        private DateTime serviceStartedTimeUTC;
        private TimeSpan ServiceUptime => DateTime.UtcNow - serviceStartedTimeUTC;

        public Dictionary<string, string> GetDiagnosticsInfo()
        {
            return new Dictionary<string, string>
            {
                { "Service Started Time UTC", serviceStartedTimeUTC.ToString() },
                { "Service Uptime", ServiceUptime.ToString() }
            };
        }

        #endregion
    }
}
