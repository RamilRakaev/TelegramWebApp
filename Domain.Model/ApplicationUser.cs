using Microsoft.AspNetCore.Identity;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        { }

        public ApplicationUser(string name, string email, string password, ApplicationUserRole role)
        {
            UserName = name;
            Email = email;
            Password = password;
            RoleId = role.Id;
            Role = role;
        }

        public ApplicationUser(string name, string email, string password, int roleId)
        {
            UserName = name;
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        public int RoleId { get; set; }
        public ApplicationUserRole Role { get; set; }
        public string Password { get; set; }
    }
}
