namespace NotificationBot.DataAccess.Entities
{
    public class UserSettings
    {
        public bool UseSleepHours { get; set; }
        public TimeSpan SleepTimeStart { get; set; }
        public TimeSpan SleepTimeEnd { get; set; }

        public User User { get; set; } = null!;
    }
}
