using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public interface IMessageGenerator
    {
        string GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset);
        string GenerateCryptoAssetInfoMessageAsync(CryptoAssetViewModel cryptoAsset);
        string GenerateFavouriteCryptoAssetsInfoMessageAsync(List<CryptoAssetViewModel> cryptoAssets);
    }
}