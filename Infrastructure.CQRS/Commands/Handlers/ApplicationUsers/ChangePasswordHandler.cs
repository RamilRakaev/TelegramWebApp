using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class ChangePasswordHandler : UserHandler, IRequestHandler<ChangePasswordCommand, ApplicationUser>
    {
        public ChangePasswordHandler(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordHandler> logger) : base(userManager, logger)
        { }

        public async Task<ApplicationUser> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);
            if (user != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
                _logger.LogInformation("Changed password");
                return user;
            }
            else
            {
                _logger.LogError("Error when changing the password");
                throw new Exception();
            }
        }
    }
}
