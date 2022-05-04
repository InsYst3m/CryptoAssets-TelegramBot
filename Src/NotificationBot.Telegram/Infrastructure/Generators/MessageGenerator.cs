using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        public async Task<string> GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset)
        {
            ArgumentNullException.ThrowIfNull(cryptoAsset, nameof(cryptoAsset));

            return await ValueTask.FromResult($"{cryptoAsset.Name}: {cryptoAsset.PriceUsd}$.");
        }
    }
}
