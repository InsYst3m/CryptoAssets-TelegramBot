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

        public async Task<List<CryptoAsset>> GetFavouriteCryptoAssetsAsync(long userId)
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
    }
}
