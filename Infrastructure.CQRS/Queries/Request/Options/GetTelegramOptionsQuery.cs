using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Queries.Request.Options
{
    public class GetTelegramOptionsQuery : IRequest<TelegramOptions>
    {
    }
}
