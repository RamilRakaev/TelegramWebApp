using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories.Configuration;

namespace Infrastructure.Repositories
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<TelegramOptions> TelegramOptions { get; set; }
        public DbSet<CalendarOptions> CalendarOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole[]
            {
                new ApplicationUserRole(){ Id = 1, Name = "admin"}
            });

            mb.Ignore<IdentityUserLogin<string>>();
            mb.Ignore<IdentityUserClaim<string>>();
            mb.Ignore<IdentityUserToken<string>>();

            mb.ApplyConfiguration(new TelegramOptionsConfiguration());
            mb.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(mb);
        }
    }
}
