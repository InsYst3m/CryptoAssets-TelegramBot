using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class BotStartCommand : IBotCommand
    {
        private const string GREETING_MESSAGE = "This is the coolest greeting message you have ever seen!";

        private readonly CommandMessage _commandMessage;
        private readonly IDataAccessService _dataAccessService;
        private readonly INotificationService _notificationService;

        public BotStartCommand(
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
            User? telegramUser = _commandMessage.Message.From;

            NotifiicationBot.Domain.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            if (user == null)
            {
                await _dataAccessService.AddUserAsync(telegramUser?.Id, chat.Id, telegramUser?.Username ?? chat.Username);
            }
            else if (!user.IsActive)
            {
                user.IsActive = true;
                user.ChatId = chat.Id;
                user.TelegramUserId = telegramUser?.Id;
                user.Username = telegramUser?.Username ?? chat.Username;

                await _dataAccessService.UpdateUserAsync(user);
            }

            await _notificationService.SendNotificationAsync(chat.Id, GREETING_MESSAGE);
        }
    }
}
