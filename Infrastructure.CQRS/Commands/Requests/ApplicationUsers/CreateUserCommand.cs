using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class CreateUserCommand : IRequest<IdentityResult>
    {
        public CreateUserCommand(ApplicationUser user)
        {
            User = user;
        }

        public CreateUserCommand(string email, string password, int roleId = 1)
        {
            Email = email ?? throw new ArgumentNullException();
            Password = password ?? throw new ArgumentNullException();
            RoleId = roleId;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
