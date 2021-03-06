using Infrastructure.CQRS.Commands.Requests.Telegram;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotService;

namespace Infrastructure.CQRS.Commands.Handlers.Telegram
{
    public class StopTelegramReceivingHandler : IRequestHandler<StopTelegramReceivingCommand, string>
    {
        public StopTelegramReceivingHandler()
        { }

        public async Task<string> Handle(StopTelegramReceivingCommand request, CancellationToken cancellationToken)
        {
            string warning = "Бот отключён";
            try
            {
                await BaseTelegramBot.StopAsync();
            }
            catch(Exception e)
            {
                warning = e.Message;
            }
            return warning;
        }
    }
}
