using NotificationBot.Telegram.Infrastructure.Parsers.Interfaces;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Parsers
{
    public class TelegramMessageParser : ITelegramMessageParser
    {
        public ParsedMessage Parse(Message message)
        {
            string content = message.Text!.Split(' ')[0];
            bool isCommand = content.StartsWith('/');

            ParsedMessage result = new()
            {
                Message = message,
                Command = isCommand ? content : null,
                CommandText = isCommand ? content[1..] : null
            };

            return result;
        }
    }
}
