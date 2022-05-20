using System.ComponentModel.DataAnnotations;

namespace NotificationBot.DataAccess.Entities
{
    public class User
    {
        public long Id { get; set; }

        public long? TelegramUserId { get; set; }
        public string? Username { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public bool IsActive { get; set; }
        public long ChatId { get; set; }

        public List<CryptoAsset> CryptoAssets { get; set; } = new List<CryptoAsset>();
        public UserSettings Settings { get; set; } = null!;
    }
}
