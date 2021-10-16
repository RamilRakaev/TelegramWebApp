using Domain.Model;
using MediatR;
using System.Collections.Generic;

namespace Infrastructure.CQRS.Commands.Requests.Options
{
    public class ChangeOptionsCommand : IRequest<Option[]>
    {
        public ChangeOptionsCommand(WebAppOptions options)
        {
            Options = options;
        }

        public WebAppOptions Options { get; set; }
    }
}
