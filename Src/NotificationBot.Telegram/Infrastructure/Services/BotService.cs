using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class BotService : IBotService
    {
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }

            var chatId = update.Message.Chat.Id;
            var message = update.Message.Text;

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId,
                text: $"You said:\n{message}",
                cancellationToken: cancellationToken);
        }
    }
}
