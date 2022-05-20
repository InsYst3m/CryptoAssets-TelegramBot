using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class BotStartCommand : IBotCommand
    {
        private readonly ParsedMessage _parsedMessage;
        private readonly IDataAccessService _dataAccessService;

        public BotStartCommand(
            ParsedMessage parsedMessage,
            IDataAccessService dataAccessService)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);

            _parsedMessage = parsedMessage;
            _dataAccessService = dataAccessService;
        }

        public async Task<string> ExecuteAsync(params string[] arguments)
        {
            Chat chat = _parsedMessage.Message.Chat;
            User? telegramUser = _parsedMessage.Message.From;

            DataAccess.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

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

            return "User was successfully added.";
        }
    }
}
