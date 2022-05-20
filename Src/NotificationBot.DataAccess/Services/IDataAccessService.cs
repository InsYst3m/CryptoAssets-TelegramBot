using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.Services
{
    public interface IDataAccessService
    {
        Task<List<CryptoAsset>> GetCryptoAssetsLookupAsync();

        /// <summary>
        /// Gets the favorite crypto assets by telegram user identifier asynchronous.
        /// </summary>
        /// <param name="telegramUserId">The telegram user identifier.</param>
        /// <returns>List of crypto assets related to user or empty list.</returns>
        Task<List<CryptoAsset>> GetFavoriteCryptoAssetsByTelegramUserIdAsync(long telegramUserId);
        Task<UserSettings?> GetUserSettingsAsync(long userId);

        Task<User> AddUserAsync(long? telegramUserId, long chatId, string? username);
        Task<bool> RemoveUserAsync(User user);
        Task<User?> GetUserByChatIdAsync(long chatId);
        Task<User?> GetUserByTelegramUserIdAsync(long telegramUserId);
        Task<User?> GetUserById(long id);
        Task<bool> UpdateUserAsync(User user);
    }
}