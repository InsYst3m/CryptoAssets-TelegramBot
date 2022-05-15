namespace NotificationBot.Telegram.Configuration
{
    /// <summary>
    /// Describes settings from appsettings json configuration.
    /// </summary>
    public class NotificationsSettings
    {
        public long StartHourUTC { get; set; }
        public long EndHourUTC { get; set; }

        // TODO: temp
        public long ChatId { get; set; }
    }
}
