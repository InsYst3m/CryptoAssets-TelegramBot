using NotificationBot.DataAccess.Entities;
using System.Text;

namespace NotificationBot.Telegram.Infrastructure.Generators
{
    public class MessageGenerator : IMessageGenerator
    {
        public async Task<string> GenerateCryptoAssetsMessageAsync(List<CryptoAsset> cryptoAssets)
        {
            if (cryptoAssets.Any())
            {
                StringBuilder stringBuilder = new("Favorite Crypto Assets Status:");
                stringBuilder.AppendLine();

                foreach (CryptoAsset asset in cryptoAssets)
                {
                    stringBuilder.AppendLine(asset.Name);
                }

                return stringBuilder.ToString();
            }

            return await ValueTask.FromResult("User does not have any favorite crypto assets yet");
        }
    }
}
