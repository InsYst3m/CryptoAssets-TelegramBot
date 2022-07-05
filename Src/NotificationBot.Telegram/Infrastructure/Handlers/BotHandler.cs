using NotificationBot.Telegram.Infrastructure.Commands;
using NotificationBot.Telegram.Infrastructure.Commands.Factory;
using NotificationBot.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Handlers
{
    public class BotHandler : IBotHandler
    {
        private readonly IBotCommandFactory _botCommandFactory;

        public BotHandler(
            IBotCommandFactory botCommandFactory)
        {
            ArgumentNullException.ThrowIfNull(botCommandFactory);

            _botCommandFactory = botCommandFactory;
        }

        #region IBotHandlers Implementation

        /// <summary>
        /// Handles service errors.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
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
                UpdateType.CallbackQuery => OnCallbackQueryReceived(botClient, update.CallbackQuery!, cancellationToken),
                _ => Task.CompletedTask
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

        public async Task HandlePeriodicTimerTickAsync(ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            try
            {
                IBotCommand command = _botCommandFactory.GetOrCreatePeriodicNotificationCommand();

                await command.ExecuteAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(botClient, ex, cancellationToken);
            }
        }

        #endregion

        #region Internal Events Implementation

        /// <summary>
        /// Waits for user keyboard callback.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="callbackQuery"></param>
        /// <param name="cancellationToken"></param>
        private async Task OnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await OnMessageReceivedAsync(botClient, callbackQuery.Message!, cancellationToken);
        }

        private async Task OnMessageReceivedAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            CommandMessage commandMessage = CommandMessage.Parse(message);

            if (!string.IsNullOrEmpty(commandMessage.Command))
            {
                await OnCommandReceivedAsync(commandMessage);
            }
        }

        private async Task OnCommandReceivedAsync(CommandMessage commandMessage)
        {
            IBotCommand? botCommand = await _botCommandFactory.GetOrCreateAsync(commandMessage);

            if (botCommand is not null)
            {
                await botCommand.ExecuteAsync();
            }
        }

        #endregion
    }
}
