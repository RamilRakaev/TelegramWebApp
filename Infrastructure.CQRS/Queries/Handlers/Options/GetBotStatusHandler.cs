using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotService;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    public class GetBotStatusHandler : IRequestHandler<GetBotStatusQuery, string>
    {
        public Task<string> Handle(GetBotStatusQuery request, CancellationToken cancellationToken)
        {
            if (AbstractTelegramBot.IsIncluded())
            {
                return Task.FromResult("Бот включён");
            }
            else
            {
                return Task.FromResult("Бот выключен");
            }
        }
    }
}
