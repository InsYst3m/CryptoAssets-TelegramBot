using NotificationBot.Telegram.Infrastructure.ViewModels;
using System.Globalization;
using System.Text;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        private readonly NumberFormatInfo _numberFormatInfo;

        public MessageGenerator()
        {
            _numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            _numberFormatInfo.NumberGroupSeparator = " ";
        }

        public string GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            return $"{cryptoAsset.Name}: {cryptoAsset.CurrentPriceUsd.ToString("#,0.0000", _numberFormatInfo)}$";
        }

        public string GenerateCryptoAssetInfoMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            StringBuilder sb = new($"Token: {cryptoAsset.Name} ({cryptoAsset.Abbreviation})" + Environment.NewLine);
            sb.AppendLine();

            sb.AppendLine($"Market Cap Rank: {cryptoAsset.Rank}");
            sb.AppendLine($"Market Capitalization: {cryptoAsset.CapitalizationUsd.ToString("#,0.0000", _numberFormatInfo)}$");
            sb.AppendLine();

            sb.AppendLine($"Current Price: {cryptoAsset.CurrentPriceUsd.ToString("#,0.0000", _numberFormatInfo)}$");
            sb.AppendLine();

            sb.AppendLine($"All Time High: {cryptoAsset.AllTimeHighPriceUsd.ToString("#,0.0000", _numberFormatInfo)}$");
            sb.AppendLine($"All Time High Change Percentage: {cryptoAsset.AllTimeHighChangePercentage}");
            sb.AppendLine();

            sb.AppendLine($"All Time Low: {cryptoAsset.AllTimeLowPriceUsd.ToString("#,0.0000", _numberFormatInfo)}$");
            sb.AppendLine($"All Time Low Change Percentage: {cryptoAsset.AllTimeLowChangePercentage}");
            sb.AppendLine();

            sb.AppendLine($"24h High: {cryptoAsset.HighTwentyFourHoursUsd.ToString("#,0.0000", _numberFormatInfo)}$");
            sb.AppendLine($"24h Low: {cryptoAsset.LowTwentyFourHoursUsd.ToString("#,0.0000", _numberFormatInfo)}$");

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
