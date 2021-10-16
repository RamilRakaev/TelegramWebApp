using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.Telegram
{
    public class StartTelegramReceivingCommand : IRequest<string>
    {
    }
}
