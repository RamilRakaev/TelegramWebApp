using MediatR;

namespace Infrastructure.CQRS.Queries.Request.Options
{
    public class GetBotStatusQuery : IRequest<string>
    {
    }
}
