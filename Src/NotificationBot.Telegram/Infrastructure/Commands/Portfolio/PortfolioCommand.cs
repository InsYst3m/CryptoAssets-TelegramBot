using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotificationBot.Telegram.Infrastructure.Commands.Portfolio
{
    public class PortfolioCommand : IBotCommand
    {
        private readonly CommandMessage _commandMessage;
        private readonly INotificationService _notificationService;

        public PortfolioCommand(
            CommandMessage commandMessage,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(commandMessage);
            ArgumentNullException.ThrowIfNull(notificationService);

            _commandMessage = commandMessage;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            InlineKeyboardMarkup keyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Add", "/createportfolio"),
                    InlineKeyboardButton.WithCallbackData("Remove", "/removeportfolio"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Update", "/updateportfolio"),
                    InlineKeyboardButton.WithCallbackData("View", "/viewportfolio")
                },
            });

            await _notificationService.SendMarkupNotificationAsync(
                _commandMessage.Message.Chat.Id,
                "Portfolio manipulation commands",
                keyboard);
        }
    }
}
