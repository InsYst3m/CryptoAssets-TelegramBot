using NotificationBot.Telegram.Infrastructure.ViewModels;
using System.Text;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        public string GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            return $"{cryptoAsset.Name}: {cryptoAsset.PriceUsd}$";
        }

        public string GenerateCryptoAssetInfoMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            return $"Command {cryptoAsset.Name}: {cryptoAsset.PriceUsd}$";
        }

        public string GenerateFavoriteCryptoAssetsInfoMessageAsync(List<CryptoAssetViewModel> cryptoAssets)
        {
            StringBuilder sb = new("Favorite Crypto Assets Prices:" + Environment.NewLine);

            foreach (var cryptoAsset in cryptoAssets)
            {
                sb.AppendLine($"{cryptoAsset.Name}: {cryptoAsset.PriceUsd}$");
            }

            return sb.ToString();
        }
    }
}
