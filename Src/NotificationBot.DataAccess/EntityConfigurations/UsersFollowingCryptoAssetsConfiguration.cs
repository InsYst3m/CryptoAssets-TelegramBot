using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.DataAccess.EntityConfigurations
{
    public class UsersFollowingCryptoAssetsConfiguration : IEntityTypeConfiguration<UsersFollowingCryptoAssets>
    {
        public void Configure(EntityTypeBuilder<UsersFollowingCryptoAssets> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CryptoAssetId });
        }
    }
}
