using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public interface IBotCommandFactory
    {
        Task<IBotCommand?> GetOrCreateAsync(CommandMessage message);
        IBotCommand GetOrCreatePeriodicNotificationCommand();
    }
}
