using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class BotStopCommand : IBotCommand
    {
        private const string OPERATION_COMPLETED = "Operation successfully completed.";

        private readonly CommandMessage _commandMessage;
        private readonly IDataAccessService _dataAccessService;
        private readonly INotificationService _notificationService;

        public BotStopCommand(
            CommandMessage commandMessage,
            IDataAccessService dataAccessService,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(commandMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(notificationService);

            _commandMessage = commandMessage;
            _dataAccessService = dataAccessService;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            Chat chat = _commandMessage.Message.Chat;

            NotifiicationBot.Domain.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            if (user is not null)
            {
                await _dataAccessService.RemoveUserAsync(user);
            }

            await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, OPERATION_COMPLETED);
        }
    }
}
