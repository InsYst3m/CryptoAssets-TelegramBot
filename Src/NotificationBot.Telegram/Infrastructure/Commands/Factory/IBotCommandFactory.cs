using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Factory
{
	public interface IBotCommandProcessorFactory
	{
		/// <summary>
		/// Creates command by the provided <see cref="Command.CommandType"/>.
		/// </summary>
		/// <param name="command"></param>
		/// <returns>
		/// Returns new instance of the <see cref="IBotCommandProcessor"/> type.
		/// </returns>
		IBotCommandProcessor Create(Command command);
		//IBotCommandProcessor GetOrCreatePeriodicNotificationCommand();
	}
}
