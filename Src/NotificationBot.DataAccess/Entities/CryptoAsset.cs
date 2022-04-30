namespace NotificationBot.DataAccess.Entities
{
    public class CryptoAsset
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string CoinGeckoAbbreviation { get; set; } = string.Empty;

        public List<User> Users { get; set; } = new List<User>();
        public List<FavoriteCryptoAsset> FavoriteCryptoAssets { get; set; } = new List<FavoriteCryptoAsset>();
    }
}
