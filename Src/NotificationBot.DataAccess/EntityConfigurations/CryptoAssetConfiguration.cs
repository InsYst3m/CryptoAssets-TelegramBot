using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.EntityConfigurations
{
    public class CryptoAssetConfiguration : IEntityTypeConfiguration<CryptoAsset>
    {
        public void Configure(EntityTypeBuilder<CryptoAsset> builder)
        {
            builder.Property(asset => asset.Id)
                .IsRequired()
                .UseIdentityColumn();
        }
    }
}
