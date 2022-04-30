using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.EntityConfigurations
{
    public class FavoriteCryptoAssetConfiguration : IEntityTypeConfiguration<FavoriteCryptoAsset>
    {
        public void Configure(EntityTypeBuilder<FavoriteCryptoAsset> builder)
        {
            builder
                .HasOne(fca => fca.User)
                .WithMany(user => user.FavoriteCryptoAssets)
                .HasForeignKey(fca => fca.UserId);

            builder
                .HasOne(fca => fca.CryptoAsset)
                .WithMany(ca => ca.FavoriteCryptoAssets)
                .HasForeignKey(fca => fca.CryptoAssetId);

            builder.HasKey(x => new { x.UserId, x.CryptoAssetId });

            builder.ToTable("FavoriteCryptoAssets");
        }
    }
}
