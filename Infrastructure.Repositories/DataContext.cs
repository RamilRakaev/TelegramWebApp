using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramOptions> TelegramOptions { get; set; }
    }
}
