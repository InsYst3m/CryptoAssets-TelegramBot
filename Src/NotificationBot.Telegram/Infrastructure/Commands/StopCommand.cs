using NotificationBot.Telegram.Helpers;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class StopCommand : Command
    {
        public StopCommand(long receiverId)
            : base(ConstantsHelper.Commands.STOP, receiverId)
        {
        }
    }
}
