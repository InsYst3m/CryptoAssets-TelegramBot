using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Infrastructure.Parsers.Models
{
    public class ParsedMessage
    {
        public Message Message { get; set; } = null!;
        public string? Command { get; set; }
        public string? CommandText { get; set; }
    }
}
