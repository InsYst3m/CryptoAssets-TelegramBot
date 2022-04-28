using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public class BotService : IBotService
    {
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
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

            // Echo message
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId,
                text: $"You said:\n{message}. ChatId: {chatId}.",
                cancellationToken: cancellationToken);
        }

        public async Task SendNotificationAsync(ITelegramBotClient botClient, string chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, text: "btc price: 30000$", cancellationToken: cancellationToken);

            await Task.CompletedTask;
        }
    }
}
