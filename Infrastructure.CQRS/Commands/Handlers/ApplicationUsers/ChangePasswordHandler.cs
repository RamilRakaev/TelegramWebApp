using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class ChangePasswordHandler : UserHandler, IRequestHandler<ChangePasswordCommand, ApplicationUser>
    {
        public ChangePasswordHandler(UserManager<ApplicationUser> userManager) : base(userManager)
        { }

        public async Task<ApplicationUser> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
                return user;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
