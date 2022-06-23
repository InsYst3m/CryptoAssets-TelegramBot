using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class NotSupportedCommand : IBotCommand
    {
        private const string COMMAND_IS_NOT_SUPPORTED = "Command is not supported.";

        private readonly ParsedMessage _parsedMessage;
        private readonly INotificationService _notificationService;

        public NotSupportedCommand(
            ParsedMessage parsedMessage,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(notificationService);

            _parsedMessage = parsedMessage;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync(params string[] arguments)
        {
            await _notificationService.SendNotificationAsync(
                _parsedMessage.Message.Chat.Id,
                COMMAND_IS_NOT_SUPPORTED);
        }
    }
}
