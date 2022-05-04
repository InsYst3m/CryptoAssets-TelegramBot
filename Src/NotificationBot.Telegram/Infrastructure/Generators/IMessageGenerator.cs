using NotificationBot.DataAccess.Entities;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public interface IMessageGenerator
    {
        Task<string> GenerateCryptoAssetsMessageAsync(CryptoAssetViewModel cryptoAsset);
    }
}