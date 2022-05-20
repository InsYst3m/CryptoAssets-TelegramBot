using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class BotStopCommand : IBotCommand
    {
        private readonly ParsedMessage _parsedMessage;
        private readonly IDataAccessService _dataAccessService;

        public BotStopCommand(
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

            DataAccess.Entities.User? user = await _dataAccessService.GetUserByChatIdAsync(chat.Id);

            if (user is not null)
            {
                await _dataAccessService.RemoveUserAsync(user);
            }

            return "Successfully completed.";
        }
    }
}
