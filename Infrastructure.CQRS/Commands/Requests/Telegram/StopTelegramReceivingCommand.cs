using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.Telegram
{
    public class StopTelegramReceivingCommand : IRequest<string>
    {
    }
}
