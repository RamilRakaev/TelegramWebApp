using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class UserHandler
    {
        protected readonly UserManager<ApplicationUser> _db;

        public UserHandler(UserManager<ApplicationUser> db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
        }
    }
}