namespace NotificationBot.Telegram.Services.Interfaces
{
    public interface IDiagnosticService
    {
        Dictionary<string, string> GetDiagnosticsInfo();
    }
}
