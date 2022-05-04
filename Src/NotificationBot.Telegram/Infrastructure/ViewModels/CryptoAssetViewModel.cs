namespace NotificationBot.Telegram.Infrastructure.ViewModels
{
    public class CryptoAssetViewModel
    {
        public string Name { get; set; }
        public decimal PriceUsd { get; set; }

        public CryptoAssetViewModel(string name, decimal priceUsd)
        {
            Name = name;
            PriceUsd = priceUsd;
        }
    }
}
