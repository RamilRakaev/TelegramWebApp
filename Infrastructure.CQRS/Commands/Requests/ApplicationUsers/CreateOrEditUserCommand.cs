using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class CreateOrEditUserCommand : IRequest<IdentityResult>
    {
        public CreateOrEditUserCommand(ApplicationUser user)
        {
            User = user;
        }

        public ApplicationUser User { get; set; }
    }
}
