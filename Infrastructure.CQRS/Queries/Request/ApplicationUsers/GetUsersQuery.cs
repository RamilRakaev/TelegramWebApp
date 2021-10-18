using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Queries.Request.ApplicationUsers
{
    public class GetUsersQuery : IRequest<ApplicationUser[]>
    {
    }
}
