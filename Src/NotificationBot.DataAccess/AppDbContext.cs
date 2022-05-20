using Microsoft.EntityFrameworkCore;
using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.EntityConfigurations;

namespace NotificationBot.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<CryptoAsset> CryptoAssets { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserSettings> UserSettings { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CryptoAssetConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<User>()
                .HasMany(x => x.CryptoAssets)
                .WithMany(x => x.Users)
                .UsingEntity(linkEntity =>
                {
                    linkEntity.ToTable("UsersCryptoAssets");
                });

            modelBuilder.Entity<User>()
                .HasOne(x => x.Settings)
                .WithOne(x => x.User)
                .HasForeignKey<UserSettings>(x => x.UserId);
        }
    }
}