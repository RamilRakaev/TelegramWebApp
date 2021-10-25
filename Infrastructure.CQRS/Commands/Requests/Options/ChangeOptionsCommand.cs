using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.Options
{
    public class ChangeOptionsCommand : IRequest<Option[]>
    {
        public ChangeOptionsCommand(WebAppOptionsEnum options)
        {
            Options = options;
        }

        public WebAppOptionsEnum Options { get; set; }
    }
}
