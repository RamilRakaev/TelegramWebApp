using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class UserHandler
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger _logger;

        public UserHandler(UserManager<ApplicationUser> db, ILogger logger)
        {
            _userManager = db ?? throw new ArgumentNullException(nameof(UserManager<ApplicationUser>));
            _logger = logger;
        }
    }
}