using MediatR;
using TelegramBotService;

namespace Infrastructure.CQRS.Commands.Requests.Telegram
{
    public class StartTelegramReceivingCommand : IRequest<string>
    {
        public StartTelegramReceivingCommand(Mode mode)
        {
            Mode = mode;
        }

        public Mode Mode { get; set; }
    }
}
