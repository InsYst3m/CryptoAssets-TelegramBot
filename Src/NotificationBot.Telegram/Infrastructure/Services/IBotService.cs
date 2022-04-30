using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public interface IBotService : IBotHandlers
    {
        void Start(CancellationToken cancellationToken);
        Task SetupPeriodicNotifications(CancellationToken cancellationToken);
    }
}