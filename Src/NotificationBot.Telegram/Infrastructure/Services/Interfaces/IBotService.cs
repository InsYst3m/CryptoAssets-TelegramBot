using NotificationBot.Telegram.Infrastructure.Handlers;
using NotificationBot.Telegram.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface IBotService : IDiagnosticService
    {
        void Start(CancellationToken cancellationToken);
    }
}