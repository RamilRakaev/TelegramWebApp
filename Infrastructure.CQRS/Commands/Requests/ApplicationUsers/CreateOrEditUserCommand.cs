using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class CreateOrEditUserCommand : IRequest<IdentityResult>
    {
        public CreateOrEditUserCommand(ApplicationUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(ApplicationUser));
        }

        public ApplicationUser User { get; set; }
    }
}
