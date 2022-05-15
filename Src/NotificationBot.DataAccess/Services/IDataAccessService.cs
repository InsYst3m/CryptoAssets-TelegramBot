using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.Services
{
    public interface IDataAccessService
    {
        Task<List<CryptoAsset>> GetCryptoAssetsLookupAsync();
        Task<List<CryptoAsset>> GetFavouriteCryptoAssetsAsync(long userId);
    }
}