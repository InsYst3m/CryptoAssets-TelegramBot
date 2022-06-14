using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifiicationBot.Domain.Entities;

namespace NotificationBot.DataAccess.EntityConfigurations
{
    public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .IsRequired()
                .UseIdentityColumn();
        }
    }
}
