using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class RemoveUserCommand : IRequest<Domain.Model.ApplicationUser>
    {
        public int Id { get; set; }
    }
}
