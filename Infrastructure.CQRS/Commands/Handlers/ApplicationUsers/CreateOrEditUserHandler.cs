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
            var user = await _userManager.FindByIdAsync(request.User.Id.ToString());
            if(user == null)
            {
                var password = request.User.Password;
                request.User.Password = null;
                return await _userManager.CreateAsync(request.User, password);
            }
            else
            {
                user.UserName = request.User.UserName;
                user.Email = request.User.Email;
                user.RoleId = request.User.RoleId;
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.User.Password);
                return await _userManager.UpdateAsync(user);
            }
        }
    }
}
