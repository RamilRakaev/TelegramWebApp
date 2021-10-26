using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class RemoveUserHandler : UserHandler, IRequestHandler<RemoveUserCommand, ApplicationUser>
    {
        public RemoveUserHandler(UserManager<ApplicationUser> db, ILogger<RemoveUserHandler> logger) : base(db, logger)
        { }

        public async Task<ApplicationUser> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                throw new NullReferenceException();
            }
            _logger.LogInformation("Deleting user");
            await _userManager.DeleteAsync(user);
            return user;
        }
    }
}
