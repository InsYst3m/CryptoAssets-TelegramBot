using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationBot.DataAccess.Entities;

namespace NotificationBot.DataAccess.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.Id)
                .IsRequired()
                .UseIdentityColumn();

            builder.Property(user => user.Email)
                .IsRequired();

            builder
                .HasMany(x => x.CryptoAssets)
                .WithMany(x => x.Users)
                .UsingEntity<FavoriteCryptoAsset>();
        }
    }
}
