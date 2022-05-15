namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public interface IBotCommand
    {
        /// <summary>
        /// Executes bot command asynchronous.
        /// </summary>
        /// <param name="arguments">Telegram bot command arguments.</param>
        /// <returns>Returns generated text message.</returns>
        Task<string> ExecuteAsync(params string[] arguments);
    }
}
