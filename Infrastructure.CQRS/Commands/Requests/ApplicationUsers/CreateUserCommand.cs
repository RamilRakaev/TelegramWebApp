using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class CreateUserCommand : IRequest<IdentityResult>
    {
        public Domain.Model.ApplicationUser User { get; set; }

        public CreateUserCommand(Domain.Model.ApplicationUser user)
        {
            User = user;
        }
    }
}
