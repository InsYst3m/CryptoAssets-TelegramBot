using NotificationBot.Telegram.Infrastructure.Commands;

using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Services.Interfaces
{
	public interface IMessageParser
	{
		/// <summary>
		/// Parses <see cref="Message"/> received from the telegram bot.
		/// </summary>
		/// <param name="message"></param>
		/// <returns>
		/// Returns instance of the <see cref="Command"/>.
		/// </returns>
		Task<Command> ParseAsync(Message message);
	}
}
