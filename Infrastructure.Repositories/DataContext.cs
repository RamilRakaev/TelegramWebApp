using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramOptions> TelegramOptions { get; set; }
    }
}
