using Microsoft.AspNetCore.Identity;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public string Password { get; set; }
    }
}
