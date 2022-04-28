namespace NotificationBot.Telegram.Configuration
{
    /// <summary>
    /// Describes settings from appsettings json configuration.
    /// </summary>
    public class BotSettings
    {
        public string? Token { get; set; }

        // TODO: temp
        public string? ChatId { get; set; }
    }
}
