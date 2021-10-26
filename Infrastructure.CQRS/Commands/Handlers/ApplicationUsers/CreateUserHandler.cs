using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class CreateUserHandler : UserHandler, IRequestHandler<CreateUserCommand, IdentityResult>
    {
        public CreateUserHandler(UserManager<ApplicationUser> userManager, ILogger<CreateUserHandler> logger) : base(userManager, logger)
        { }

        public async Task<IdentityResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new user");
            return await _userManager.CreateAsync(request.User);
        }
    }
}
