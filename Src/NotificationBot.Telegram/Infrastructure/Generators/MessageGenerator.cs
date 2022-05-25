using NotificationBot.Telegram.Infrastructure.ViewModels;
using System.Globalization;
using System.Text;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        public string GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            return $"{cryptoAsset.Name}: {cryptoAsset.CurrentPriceUsd:N1}$";
        }

        public string GenerateCryptoAssetInfoMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            StringBuilder sb = new($"Token: {cryptoAsset.Name} ({cryptoAsset.Abbreviation})" + Environment.NewLine);
            sb.AppendLine();

            sb.AppendLine($"Market Cap Rank: {cryptoAsset.Rank}");
            sb.AppendLine($"Market Capitalization: {cryptoAsset.CapitalizationUsd:N1}$");
            sb.AppendLine();

            sb.AppendLine($"Current Price: {cryptoAsset.CurrentPriceUsd:N1}$");
            sb.AppendLine();

            sb.AppendLine($"All Time High: {cryptoAsset.AllTimeHighPriceUsd:N1}$");
            sb.AppendLine($"All Time High Change Percentage: {cryptoAsset.AllTimeHighChangePercentage}");
            sb.AppendLine();

            sb.AppendLine($"All Time Low: {cryptoAsset.AllTimeLowPriceUsd:N1}$");
            sb.AppendLine($"All Time Low Change Percentage: {cryptoAsset.AllTimeLowChangePercentage}");
            sb.AppendLine();

            sb.AppendLine($"24h High: {cryptoAsset.HighTwentyFourHoursUsd:N1}$");
            sb.AppendLine($"24h Low: {cryptoAsset.LowTwentyFourHoursUsd:N1}$");

            return sb.ToString();
        }

        public string GenerateFavoriteCryptoAssetsInfoMessageAsync(List<CryptoAssetViewModel> cryptoAssets)
        {
            StringBuilder sb = new("Favorite Tokens Prices:" + Environment.NewLine);

            foreach (CryptoAssetViewModel cryptoAsset in cryptoAssets)
            {
                sb.AppendLine(GenerateCryptoAssetsMessageAsync(cryptoAsset));
            }

            return sb.ToString();
        }
    }
}
