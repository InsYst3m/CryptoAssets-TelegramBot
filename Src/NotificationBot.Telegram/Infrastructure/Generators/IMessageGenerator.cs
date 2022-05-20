using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public interface IMessageGenerator
    {
        string GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset);
        string GenerateCryptoAssetInfoMessageAsync(CryptoAssetViewModel cryptoAsset);
        string GenerateFavoriteCryptoAssetsInfoMessageAsync(List<CryptoAssetViewModel> cryptoAssets);
    }
}