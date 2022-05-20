using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.Services
{
    public interface IDataAccessService
    {
        Task<List<CryptoAsset>> GetCryptoAssetsLookupAsync();
        Task<List<CryptoAsset>> GetFavouriteCryptoAssetsAsync(long userId);
        Task<UserSettings?> GetUserSettingsAsync(long userId);

        Task<User> AddUserAsync(long? telegramUserId, long chatId, string? username);
        Task<bool> RemoveUserAsync(User user);
        Task<User?> GetUserByChatIdAsync(long chatId);
        Task<User?> GetUserByTelegramUserIdAsync(long telegramUserId);
        Task<User?> GetUserById(long id);
        Task<bool> UpdateUserAsync(User user);
    }
}