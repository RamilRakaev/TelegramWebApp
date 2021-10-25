using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Queries.Request.Options
{
    public class GetWebAppOptionsQuery : IRequest<WebAppOptionsEnum>
    {
    }
}
