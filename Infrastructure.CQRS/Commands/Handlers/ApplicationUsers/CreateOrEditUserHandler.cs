using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class CreateOrEditUserHandler : UserHandler, IRequestHandler<CreateOrEditUserCommand, IdentityResult>
    {
        public CreateOrEditUserHandler(UserManager<ApplicationUser> userManager, ILogger<CreateOrEditUserHandler> logger) : base(userManager, logger)
        { }

        public async Task<IdentityResult> Handle(CreateOrEditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                var newUser = new ApplicationUser() { UserName = request.Email, Email = request.Email, RoleId = request.RoleId };
                return await _userManager.CreateAsync(newUser, request.Password);
            }
            else
            {
                user.Email = request.Email;
                user.UserName = request.Email;
                user.RoleId = request.RoleId;
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
                _logger.LogInformation("Edited user password");
                return await _userManager.UpdateAsync(user);
            }
        }
    }
}
