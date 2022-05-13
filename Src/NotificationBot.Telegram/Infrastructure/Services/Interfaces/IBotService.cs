using NotificationBot.Telegram.Services;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface IBotService : IBotHandlers, IDiagnosticService
    {
        void Start(CancellationToken cancellationToken);
        Task SetupPeriodicNotifications(CancellationToken cancellationToken);
    }
}