using NotificationBot.Telegram.Infrastructure.Parsers.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public interface IBotCommandFactory
    {
        Task<IBotCommand?> GetOrCreateAsync(ParsedMessage message);
        IBotCommand GetOrCreatePeriodicNotificationCommand();
    }
}
