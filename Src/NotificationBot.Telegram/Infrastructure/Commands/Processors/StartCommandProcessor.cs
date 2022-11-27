using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

namespace NotificationBot.Telegram.Infrastructure.Commands.Processors
{
    public class StartCommandProcessor : IBotCommandProcessor
    {
        private const string GREETING_MESSAGE = "This is the coolest greeting message you have ever seen!";

        private readonly Command _commandInfo;
        private readonly INotificationService _notificationService;

        public StartCommandProcessor(
            Command commandInfo,
            INotificationService notificationService)
        {
            _commandInfo = commandInfo ?? throw new ArgumentNullException(nameof(commandInfo));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public async Task ProcessAsync()
        {
            //NotifiicationBot.Domain.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            //if (user == null)
            //{
            //    await _dataAccessService.AddUserAsync(telegramUser?.Id, chat.Id, telegramUser?.Username ?? chat.Username);
            //}
            //else if (!user.IsActive)
            //{
            //    user.IsActive = true;
            //    user.ChatId = chat.Id;
            //    user.TelegramUserId = telegramUser?.Id;
            //    user.Username = telegramUser?.Username ?? chat.Username;

            //    await _dataAccessService.UpdateUserAsync(user);
            //}

            await _notificationService.SendNotificationAsync(_commandInfo.ReceiverId, GREETING_MESSAGE);
        }
    }
}
