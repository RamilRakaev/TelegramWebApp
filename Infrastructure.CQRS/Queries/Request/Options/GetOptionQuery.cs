using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Queries.Request.Options
{
    public class GetOptionQuery : IRequest<Option>
    {
        public string PropertyName { get; set; }
    }
}
