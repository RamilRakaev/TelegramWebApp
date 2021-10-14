using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Request.User
{
    public class RemoveUserCommand : IRequest<ApplicationUser>
    {
        public int Id { get; set; }
    }
}
