namespace NotificationBot.Telegram.Services
{
    public interface IDiagnosticService
    {
        Dictionary<string, string> GetDiagnosticsInfo();
    }
}
