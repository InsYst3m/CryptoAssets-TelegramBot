namespace NotificationBot.Telegram.Configuration
{
    public class NotificationsSettings
    {
        public long StartHourUTC { get; set; }
        public long EndHourUTC { get; set; }
        public long IntervalInHours { get; set; }
    }
}
