using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.CQRS.Commands.Request.User
{
    public class CreateUserCommand : IRequest<IdentityResult>
    {
        public ApplicationUser User { get; set; }

        public CreateUserCommand(ApplicationUser user)
        {
            User = user;
        }
    }
}
