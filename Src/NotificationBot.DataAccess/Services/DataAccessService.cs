using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NotificationBot.DataAccess.Entities;

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

        /// <inheritdoc cref="IDataAccessService.GetFavoriteCryptoAssetsByTelegramUserIdAsync(long)" />
        public async Task<List<CryptoAsset>> GetFavoriteCryptoAssetsByTelegramUserIdAsync(long userId)
        {
            using AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User? user = context.Users.Include(x => x.CryptoAssets).FirstOrDefault(x => x.Id == userId);

            if (user is not null)
            {
                return user.CryptoAssets;
            }

            return new List<CryptoAsset>();
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
    }
}
