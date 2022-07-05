using NotifiicationBot.Domain.Entities;

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
        Task<List<CryptoAsset>> GetFollowedCryptoAssetsByTelegramUserIdAsync(long telegramUserId);

        /// <summary>
        /// Gets the list of users with <see cref="UserSettings.UsePeriodicNotifications"/> flag
        /// to send periodic notifications asynchronous.
        /// </summary>
        /// <returns>List of users or empty list.</returns>
        Task<List<User>> GetUsersToSendPeriodicNotificationsAsync();
        Task<UserSettings?> GetUserSettingsAsync(long userId);

        Task<User> AddUserAsync(long? telegramUserId, long chatId, string? username);
        Task<bool> RemoveUserAsync(User user);
        Task<User?> GetUserByChatIdAsync(long chatId);
        Task<User?> GetUserByTelegramUserIdAsync(long telegramUserId);
        Task<User?> GetUserById(long id);
        Task<bool> UpdateUserAsync(User user);

        /// <inheritdoc cref="IDataAccessService.CreatePortfolioAsync(string, long)" />
        Task<Portfolio> CreatePortfolioAsync(string name, long userId);
    }
}