﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Services
{
    public interface IBotService
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);

        Task SendNotificationAsync(ITelegramBotClient botClient, string chatId, string message, CancellationToken cancellationToken);
    }
}