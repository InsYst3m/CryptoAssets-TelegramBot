using Microsoft.Extensions.Hosting;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.HostedServices
{
    public class TelegramBotHostedService : IHostedService
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
            _botService.Start(_tokenSource.Token);
            await _botService.SetupPeriodicNotifications(_tokenSource.Token);

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            return Task.CompletedTask;
        }

        #endregion
    }
}
