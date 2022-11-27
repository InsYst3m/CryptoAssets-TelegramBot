namespace NotificationBot.Telegram.Infrastructure.Commands
{
	public abstract class Command
	{
		public string Value { get; private set; }
		public long ReceiverId { get; private set; }

		public Command(string command, long receiverId)
		{
			Value = command ?? throw new ArgumentNullException(nameof(command));

			ReceiverId = receiverId;
		}
	}
}
