using NotificationBot.Telegram.Infrastructure.Commands.Interfaces;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Models;

namespace NotificationBot.Telegram.Infrastructure.Commands.Portfolio
{
    public class PortfolioCreateCommand : IBotCommand
    {
        private const string SUCCESSFULLY_CREATED_MESSAGE = "Portfolio was successfully created.";
        private const string SPECIFY_PORTFOLIO_NAME = "Please specify portfolio name.";

        private readonly CommandMessage _commandMessage;
        private readonly INotificationService _notificationService;

        public PortfolioCreateCommand(
            CommandMessage commandMessage,
            INotificationService notificationService)
        {
            _commandMessage = commandMessage;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            if (_commandMessage.Arguments is null || 
                _commandMessage.Arguments.Length == 0)
            {
                await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, SPECIFY_PORTFOLIO_NAME);

                return;
            }

            string name = _commandMessage.Arguments[0];
            long userId = _commandMessage.Message.Chat.Id;

            //await _dataAccessService.CreatePortfolioAsync(name, userId);
            await _notificationService.SendNotificationAsync(_commandMessage.Message.Chat.Id, SUCCESSFULLY_CREATED_MESSAGE);
        }
    }
}
