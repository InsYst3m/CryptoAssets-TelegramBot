namespace NotificationBot.Telegram.Services.Interfaces
{
    public interface ITimerWrapper : IDiagnosticService, IDisposable
    {
        event EventHandler<string> PeriodicTimerEventHandler;
        bool SetupPeriodicTimer(CancellationToken cancellationToken = default);
    }
}
