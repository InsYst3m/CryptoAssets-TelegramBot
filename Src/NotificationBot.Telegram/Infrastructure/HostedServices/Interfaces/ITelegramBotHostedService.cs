using NotificationBot.Telegram.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.HostedServices.Interfaces
{
    public interface ITelegramBotHostedService : IHostedService, IDiagnosticService
    {
    }
}
