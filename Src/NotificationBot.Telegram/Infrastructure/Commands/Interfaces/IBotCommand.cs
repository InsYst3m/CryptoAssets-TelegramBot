namespace NotificationBot.Telegram.Infrastructure.Commands.Interfaces
{
	public interface IBotCommandProcessor
	{
		/// <summary>
		/// Executes bot command asynchronous.
		/// </summary>
		/// <param name="arguments">Telegram bot command arguments.</param>
		/// <returns>Returns generated text message.</returns>
		Task ProcessAsync();
	}
}
