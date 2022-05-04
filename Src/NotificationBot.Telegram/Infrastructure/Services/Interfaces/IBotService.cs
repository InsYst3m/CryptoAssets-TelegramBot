namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
    public interface IBotService : IBotHandlers
    {
        void Start(CancellationToken cancellationToken);
        Task SetupPeriodicNotifications(CancellationToken cancellationToken);
    }
}