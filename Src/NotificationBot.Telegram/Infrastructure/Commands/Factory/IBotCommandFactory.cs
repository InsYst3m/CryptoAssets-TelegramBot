namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
    public interface IBotCommandFactory
    {
        Task<IBotCommand?> GetOrCreateAsync(string command);
    }
}
