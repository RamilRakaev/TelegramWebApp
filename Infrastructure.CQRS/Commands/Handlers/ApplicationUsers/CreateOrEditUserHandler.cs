using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class CreateOrEditUserHandler : IRequestHandler<CreateOrEditUserCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateOrEditUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

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
                user.RoleId = request.RoleId;
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
                return await _userManager.UpdateAsync(user);
            }
        }
    }
}
