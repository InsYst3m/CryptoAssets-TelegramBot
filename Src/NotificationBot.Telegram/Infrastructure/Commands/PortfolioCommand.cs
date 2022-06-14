namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class PortfolioCommand : IBotCommand
    {
        public Task<string> ExecuteAsync(params string[] arguments)
        {
            // TODO: show keyboard to user
            // Add
            // Update

            return Task.FromResult("bla bla bla");
        }
    }
}
