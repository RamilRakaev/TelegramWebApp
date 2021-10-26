using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class UserHandler
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public UserHandler(UserManager<ApplicationUser> db)
        {
            _userManager = db ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
        }
    }
}