using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Configuration
{
    public class UserInitializer
    {
        private static readonly List<ApplicationUser> users = new List<ApplicationUser>()
        {
            new ApplicationUser(){ UserName = "DefaultUser", Email = "DefaultUser@gmail.com",
                    RoleId = 1},
        };

        private static readonly List<string> passwords = new List<string>()
        {
            "e23D!23df32"
        };

        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager)
        {
            int i = 0;
            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    await userManager.CreateAsync(user, passwords[i++]);
                }
            }
        }
    }
}
