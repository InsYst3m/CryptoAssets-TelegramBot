using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.DataAccess.Services
{
    public class DataAccessService : IDataAccessService
    {
        private const string CRYPTO_ASSETS_LOOKUP_CACHE_KEY = "CryptoAssetsLookupCache";

        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessService"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public DataAccessService(IDbContextFactory<AppDbContext> dbContextFactory, IMemoryCache memoryCache)
        {
            ArgumentNullException.ThrowIfNull(nameof(memoryCache));
            ArgumentNullException.ThrowIfNull(nameof(dbContextFactory));

            _dbContextFactory = dbContextFactory;
            _memoryCache = memoryCache;
        }

        public async Task<List<CryptoAsset>> GetCryptoAssetsLookupAsync()
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return await _memoryCache.GetOrCreateAsync(
                CRYPTO_ASSETS_LOOKUP_CACHE_KEY,
                async cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120);

                    return await context.CryptoAssets.ToListAsync();
                });
        }

        /// <inheritdoc cref="IDataAccessService.GetFollowedCryptoAssetsByTelegramUserIdAsync(long)" />
        public async Task<List<CryptoAsset>> GetFollowedCryptoAssetsByTelegramUserIdAsync(long telegramUserId)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User? user = context.Users
                .Include(x => x.FollowedCryptoAssets)
                .ThenInclude(x => x.CryptoAsset)
                .FirstOrDefault(x => x.ChatId == telegramUserId);

            if (user is not null)
            {
                return user.FollowedCryptoAssets
                    .Select(x => x.CryptoAsset)
                    .ToList();
            }

            return new List<CryptoAsset>();
        }

        /// <inheritdoc cref="IDataAccessService.GetUsersToSendPeriodicNotificationsAsync" />
        public async Task<List<User>> GetUsersToSendPeriodicNotificationsAsync()
        {
            List<User> result = new();

            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            result = await context.Users
                .Include(x => x.Settings)
                .Where(x => x.Settings.UsePeriodicNotifications).ToListAsync();

            return result;
        }

        public async Task<UserSettings?> GetUserSettingsAsync(long userId)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User? user = context.Users.Include(x => x.Settings).FirstOrDefault(x => x.Id == userId);

            if (user is not null)
            {
                return user.Settings;
            }

            return null;
        }

        public async Task<User> AddUserAsync(long? telegramUserId, long chatId, string? username)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User user = new()
            {
                Username = username,
                TelegramUserId = telegramUserId,
                ChatId = chatId,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public async Task<bool> RemoveUserAsync(User user)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            user.IsActive = false;

            context.Users.Update(user);
            context.SaveChanges();

            return true;
        }

        public async Task<User?> GetUserByChatIdAsync(long chatId)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return context.Users.FirstOrDefault(x => x.ChatId == chatId);
        }

        public async Task<User?> GetUserByTelegramUserIdAsync(long telegramUserId)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return context.Users.FirstOrDefault(x => x.TelegramUserId == telegramUserId);
        }

        public async Task<User?> GetUserById(long id)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return await context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            context.Users.Update(user);
            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Creates crypto assets portfolio with specified name.
        /// </summary>
        /// <param name="name">The portfolio name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Instance of <see cref="Portfolio"/> class.</returns>
        public async Task<Portfolio> CreatePortfolioAsync(string name, long userId)
        {
            Portfolio portfolio = new(name, userId);

            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            context.Portfolios.Add(portfolio);
            context.SaveChanges();

            return portfolio;
        }
    }
}
