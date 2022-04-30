using NotificationBot.DataAccess.Entities;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public interface IMessageGenerator
    {
        Task<string> GenerateCryptoAssetsMessageAsync(List<CryptoAsset> cryptoAssets);
    }
}