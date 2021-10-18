using Domain.Model;
using MediatR;
using System;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class RemoveUserCommand : IRequest<ApplicationUser>
    {
        public RemoveUserCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
