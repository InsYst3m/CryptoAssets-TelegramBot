using Telegram.Bot.Types;

namespace NotificationBot.Telegram.Models
{
	/// <summary>
	/// Represents telegram chat parsed message.
	/// </summary>
	public class CommandMessage
	{
		#region Constants

		private const string ARGUMENTS_DELIMETER = ",";

		#endregion

		public Message Message { get; set; } = null!;

		public string? Command { get; set; }

		public string[]? Arguments { get; set; }

		/// <summary>
		/// Parses telegram chat message command.
		/// </summary>
		/// <param name="message"></param>
		/// <remarks>
		/// Command format: /get {argument 1},{argument 2}
		/// </remarks>
		/// <returns>
		/// Returnes newly created instance of <see cref="CommandMessage"/> object.
		/// </returns>
		public static CommandMessage Parse(Message message)
		{
			string messageText = message.Text!;

			bool isCommand = messageText[0] == '/';
			if (isCommand)
			{
				string[] splittedCommand = messageText.Split(" ");
				string[] arguments = splittedCommand[1].Split(ARGUMENTS_DELIMETER);

				CommandMessage result = new()
				{
					Message = message,
					Command = splittedCommand[0],
					Arguments = arguments
				};

				return result;
			}

			throw new InvalidOperationException();
		}
	}
}
