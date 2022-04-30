namespace NotificationBot.DataAccess.Entities
{
    public class FavoriteCryptoAsset
    {
        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long CryptoAssetId { get; set; }
        public CryptoAsset CryptoAsset { get; set; } = null!;
    }
}
