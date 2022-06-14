namespace NotifiicationBot.Domain.Entities
{
    public class CryptoAsset
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string CoinGeckoAbbreviation { get; set; } = string.Empty;

        public IList<CryptoTransaction> CryptoTransactions { get; set; } = new List<CryptoTransaction>();
        public IList<UsersFollowingCryptoAssets> Followers { get; set; } = new List<UsersFollowingCryptoAssets>();
    }
}
