using MediatR;
using Domain.Model;

namespace Infrastructure.CQRS.Commands.Request.Options
{
    public class ChangeTelegramOptionsCommand : IRequest<TelegramOptions>
    {
        public ChangeTelegramOptionsCommand(TelegramOptions options)
        {
            Options = options;
        }

        public TelegramOptions Options { get; set; }
    }
}
