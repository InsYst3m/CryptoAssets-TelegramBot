using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class BotStopCommand : IBotCommand
    {
        private const string OPERATION_COMPLETED = "Operation successfully completed.";

        private readonly ParsedMessage _parsedMessage;
        private readonly IDataAccessService _dataAccessService;
        private readonly INotificationService _notificationService;

        public BotStopCommand(
            ParsedMessage parsedMessage,
            IDataAccessService dataAccessService,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(notificationService);

            _parsedMessage = parsedMessage;
            _dataAccessService = dataAccessService;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync(params string[] arguments)
        {
            Chat chat = _parsedMessage.Message.Chat;

            NotifiicationBot.Domain.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            if (user is not null)
            {
                await _dataAccessService.RemoveUserAsync(user);
            }

            await _notificationService.SendNotificationAsync(_parsedMessage.Message.Chat.Id, OPERATION_COMPLETED);
        }
    }
}
