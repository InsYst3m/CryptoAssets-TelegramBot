namespace NotificationBot.Telegram.Services.Interfaces
{
    public interface ITimerWrapper : IDiagnosticService, IDisposable
    {
        event EventHandler OnPeriodicTimerTickEventHandler;
        bool SetupPeriodicTimer(CancellationToken cancellationToken = default);
    }
}
