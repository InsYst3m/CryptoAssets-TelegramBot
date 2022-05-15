using NotificationBot.Telegram.Infrastructure.Commands;
using NotificationBot.Telegram.Infrastructure.Commands.Factory;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Handlers
{
    public class BotHandler : IBotHandler
    {
        private readonly IBotCommandFactory _botCommandFactory;
        private readonly INotificationService _notificationService;

        public BotHandler(
            IBotCommandFactory botCommandFactory,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(botCommandFactory);
            ArgumentNullException.ThrowIfNull(notificationService);

            _botCommandFactory = botCommandFactory;
            _notificationService = notificationService;
        }

        #region IBotHandlers Implementation

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiRequestException 
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Task handler = update.Type switch
            {
                UpdateType.Message => OnMessageReceivedAsync(botClient, update.Message!, cancellationToken),
                _ => throw new InvalidOperationException()
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(botClient, ex, cancellationToken);
            }
        }

        #endregion

        #region Internal Events Implementation

        private async Task OnMessageReceivedAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.Type is not MessageType.Text)
            {
                return;
            };

            string text = message.Text!.Split(' ')[0];

            Task<bool> action = text switch
            {
                string command when
                    command.StartsWith('/') => OnCommandReceivedAsync(botClient, message, cancellationToken),

                _ => throw new InvalidOperationException()
            };

            await action;
        }

        private async Task<bool> OnCommandReceivedAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string command = message.Text!.Split(' ')[0];
            string result = "Command is not supported.";

            IBotCommand? botCommand = await _botCommandFactory.GetOrCreateAsync(command);

            if (botCommand is not null)
            {
                result = await botCommand.ExecuteAsync(command);
            }

            return await _notificationService.SendNotificationAsync(botClient, message.Chat?.Id, result, cancellationToken);
        }

        #endregion
    }
}
