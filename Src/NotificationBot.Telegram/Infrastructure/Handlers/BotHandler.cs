using NotificationBot.Telegram.Helpers;
using NotificationBot.Telegram.Infrastructure.Commands;
using NotificationBot.Telegram.Infrastructure.Commands.Factory;
using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NotificationBot.Telegram.Infrastructure.Handlers
{
	public class BotHandler : IBotHandler
	{
		private readonly IMessageParser _messageParser;
		private readonly IBotCommandProcessorFactory _botCommandProcessorFactory;

		public BotHandler(
			IMessageParser messageParser,
			IBotCommandProcessorFactory botCommandFactory)
		{
			_messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
			_botCommandProcessorFactory = botCommandFactory ?? throw new ArgumentNullException(nameof(botCommandFactory));
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
				UpdateType.Message => OnMessageReceivedAsync(update.Message!, cancellationToken),
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
				// TODO:
				// 1) Create table with notification types
				// 2) Create link table between user and notification
				// 3) Create graph service endpoint to return user favorite assets
				// 4) Send notification to each user

				// Flow:
				// 1) Triggers periodic notification
				// 2) Bot gets all users subscribed to the periodic notification with their favorite assets
				// 3) Bot sends notification to all users

				//Command commandInfo = new GetFavoriteAssetsCommand(0);

				//IBotCommandProcessor command = _botCommandProcessorFactory.Create(commandInfo);

				//await command.ProcessAsync();
			}
			catch (Exception ex)
			{
				await HandleErrorAsync(botClient, ex, cancellationToken);
			}

			await Task.CompletedTask;
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
			await OnMessageReceivedAsync(callbackQuery.Message!, cancellationToken);
		}

		private async Task OnMessageReceivedAsync(Message message, CancellationToken cancellationToken)
		{
			Command commandInfo = await _messageParser.ParseAsync(message);

			IBotCommandProcessor command = _botCommandProcessorFactory.Create(commandInfo);

			await command.ProcessAsync();
		}

		#endregion
	}
}
