using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
    public class StopCommandProcessor : IBotCommandProcessor
    {
        private const string OPERATION_COMPLETED = "Operation successfully completed.";

        private readonly Command _commandInfo;
        private readonly INotificationService _notificationService;

        public StopCommandProcessor(
            Command commandInfo,
            INotificationService notificationService)
        {
            _commandInfo = commandInfo ?? throw new ArgumentNullException(nameof(commandInfo));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public async Task ProcessAsync()
        {
            //NotifiicationBot.Domain.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            //if (user is not null)
            //{
            //    await _dataAccessService.RemoveUserAsync(user);
            //}

            await _notificationService.SendNotificationAsync(_commandInfo.ReceiverId, OPERATION_COMPLETED);
        }
    }
}
