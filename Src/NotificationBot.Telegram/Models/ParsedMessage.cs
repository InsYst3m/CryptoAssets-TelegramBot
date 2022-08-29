using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Models
{
    /// <summary>
    /// Represents telegram chat parsed message.
    /// </summary>
    public class CommandMessage
    {
        public Message Message { get; set; } = null!;

        public string CommandType CommandType {get;set;}
        public string? Command { get; set; }


        public string[]? Arguments { get; set; }

        /// <summary>
        /// Parses telegram chat message command.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returnes newly created instance of <see cref="CommandMessage"/> object.</returns>
        public static CommandMessage Parse(Message message)
        {
            string[] content = message.Text!.Split(' ');

            bool isCommand = content[0].StartsWith('/');

            CommandMessage result = new()
            {
                Message = message,
                Command = isCommand ? content[0] : null,
                Arguments = content[1..]
            };

            return result;
        }
    }
}
