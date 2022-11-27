using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Handlers
{
	public interface IBotHandler
	{
		Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
		Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
		Task HandlePeriodicTimerTickAsync(ITelegramBotClient botClient, CancellationToken cancellationToken);
	}
}