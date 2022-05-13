using NotificationBot.Telegram.Services;

namespace NotificationBot.Telegram.Infrastructure.HostedServices.Interfaces
{
    public interface ITelegramBotHostedService : IHostedService, IDiagnosticService
    {
    }
}
