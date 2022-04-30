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

        public async Task<List<CryptoAsset>> GetCryptoAssetsLookup()
        {
            AppDbContext context = await _dbContextFactory.CreateDbContextAsync();

            return await context.CryptoAssets.ToListAsync();
        }

        public async Task<List<CryptoAsset>> GetFavoriteCryptoAssets(string userId)
        {
            return new List<CryptoAsset>();
        }
    }
}
