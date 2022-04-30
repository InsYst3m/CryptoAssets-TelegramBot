using System.ComponentModel.DataAnnotations;

namespace NotificationBot.DataAccess.Entities
{
    public class User
    {
        public long Id { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public List<CryptoAsset> CryptoAssets { get; set; } = new List<CryptoAsset>();
    }
}
