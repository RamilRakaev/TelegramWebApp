using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.Configuration
{
    public class TelegramOptionsConfiguration : IEntityTypeConfiguration<TelegramOptions>
    {
        public void Configure(EntityTypeBuilder<TelegramOptions> builder)
        {
            builder
                .HasMany(o => o.Users)
                .WithOne(u => u.TelegramOptions)
                .HasForeignKey(u => u.TelegramOptionsId);
        }
    }
}
