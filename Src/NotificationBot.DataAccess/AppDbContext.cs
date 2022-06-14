using Microsoft.EntityFrameworkCore;
using NotificationBot.DataAccess.EntityConfigurations;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<CryptoAsset> CryptoAssets { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserSettings> UsersSettings { get; set; } = null!;
        public DbSet<Portfolio> Portfolios { get; set; } = null!;
        public DbSet<CryptoTransaction> CryptoTransactions { get; set; } = null!;
        public DbSet<UsersFollowingCryptoAssets> UsersFollowingCryptoAssets { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CryptoAssetConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new CryptoTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersFollowingCryptoAssetsConfiguration());

            modelBuilder
                .Entity<User>()
                .HasOne(user => user.Settings)
                .WithOne(userSettings => userSettings.User)
                .HasForeignKey<UserSettings>(x => x.UserId);

            modelBuilder
                .Entity<User>()
                .HasMany(user => user.Portfolios)
                .WithOne(portfolio => portfolio.User)
                .HasForeignKey(portfolio => portfolio.UserId);

            modelBuilder
                .Entity<Portfolio>()
                .HasMany(portfolio => portfolio.CryptoTransactions)
                .WithOne(cryptoTransaction => cryptoTransaction.Portfolio)
                .HasForeignKey(cryptoTransaction => cryptoTransaction.PortfolioId);

            modelBuilder
                .Entity<CryptoAsset>()
                .HasMany(cryptoAsset => cryptoAsset.CryptoTransactions)
                .WithOne(cryptoTransaction => cryptoTransaction.CryptoAsset)
                .HasForeignKey(cryptoTransaction => cryptoTransaction.CryptoAssetId);

            modelBuilder
                .Entity<UsersFollowingCryptoAssets>()
                .HasOne(x => x.User)
                .WithMany(user => user.FollowedCryptoAssets)
                .HasForeignKey(x => x.UserId);

            modelBuilder
                .Entity<UsersFollowingCryptoAssets>()
                .HasOne(x => x.CryptoAsset)
                .WithMany(cryptoAsset => cryptoAsset.Followers)
                .HasForeignKey(x => x.CryptoAssetId);
        }
    }
}