using NotificationBot.DataAccess.Entities;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        public Task<string> GenerateCryptoAssetsMessageAsync(List<CryptoAsset> cryptoAssets)
        {
            throw new NotImplementedException();
        }
    }
}
