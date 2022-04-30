using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.Services
{
    public interface IDataAccessService
    {
        Task<List<CryptoAsset>> GetCryptoAssetsLookup();
        Task<List<CryptoAsset>> GetFavoriteCryptoAssets(string userId);
    }
}