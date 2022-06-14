namespace NotifiicationBot.Domain.Entities
{
    public class CryptoTransaction
    {
        public long Id { get; set; }

        public long PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; } = null!;

        public long CryptoAssetId { get; set; }
        public CryptoAsset CryptoAsset { get; set; } = null!;

        public decimal Amount { get; set; }
        public decimal BuyPrice { get; set; }

        //public decimal TotalBuyPrice => Amount * BuyPrice;
        //public decimal TotalPrice => Amount * CurrentPrice;
        //public decimal Profit => TotalPrice - TotalBuyPrice;
        //public decimal ProfitPercentage => (TotalPrice - TotalBuyPrice) / TotalBuyPrice * 100;
    }
}
