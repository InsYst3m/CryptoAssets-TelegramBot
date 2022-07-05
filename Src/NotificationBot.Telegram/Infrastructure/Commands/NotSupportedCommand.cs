using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class NotSupportedCommand : IBotCommand
    {
        private const string COMMAND_IS_NOT_SUPPORTED = "Command is not supported.";

        private readonly CommandMessage _commandMessage;
        private readonly INotificationService _notificationService;

        public NotSupportedCommand(
            CommandMessage commandMessage,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(commandMessage);
            ArgumentNullException.ThrowIfNull(notificationService);

            _commandMessage = commandMessage;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            await _notificationService.SendNotificationAsync(
                _commandMessage.Message.Chat.Id,
                COMMAND_IS_NOT_SUPPORTED);
        }
    }
}
