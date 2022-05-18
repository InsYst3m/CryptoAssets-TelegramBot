using Microsoft.EntityFrameworkCore;
using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public DataAccessService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<List<CryptoAsset>> GetCryptoAssetsLookupAsync()
        {
            // TODO: add cache

            AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return await context.CryptoAssets.ToListAsync();
        }

        public async Task<List<CryptoAsset>> GetFavouriteCryptoAssetsAsync(long userId)
        {
            AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User? user = context.Users.Include(x => x.CryptoAssets).FirstOrDefault(x => x.Id == userId);

            if (user is not null)
            {
                return user.CryptoAssets;
            }

            return new List<CryptoAsset>();
        }

        public async Task<UserSettings?> GetUserSettingsAsync(long userId)
        {
            AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            User? user = context.Users.Include(x => x.Settings).FirstOrDefault(x => x.Id == userId);

            if (user is not null)
            {
                return user.Settings;
            }

            return null;
        }
    }
}
