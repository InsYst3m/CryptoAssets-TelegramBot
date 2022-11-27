using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
	public class NotSupportedCommandProcessor : IBotCommandProcessor
	{
		private const string COMMAND_IS_NOT_SUPPORTED = "Command is not supported.";

		private readonly Command _command;
		private readonly INotificationService _notificationService;

		public NotSupportedCommandProcessor(
			Command command,
			INotificationService notificationService)
		{
			_command = command ?? throw new ArgumentNullException(nameof(command));
			_notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
		}

		public async Task ProcessAsync()
		{
			await _notificationService.SendNotificationAsync(
				_command.ReceiverId,
				COMMAND_IS_NOT_SUPPORTED);
		}
	}
}
