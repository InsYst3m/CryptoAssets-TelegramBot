using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Parsers.Interfaces
{
    public interface ITelegramMessageParser
    {
        ParsedMessage Parse(Message message);
    }
}
