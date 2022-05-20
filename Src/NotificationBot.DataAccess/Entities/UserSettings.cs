namespace NotificationBot.DataAccess.Entities
{
    public class UserSettings
    {
        public long Id { get; set; }
        public bool UsePeriodicNotifications { get; set; }
        public bool UseSleepHours { get; set; }

        public TimeSpan? SleepTimeStart { get; set; }
        public TimeSpan? SleepTimeEnd { get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
