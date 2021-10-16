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
        private readonly TelegramBot _bot;

        public Task<string> Handle(StopTelegramReceivingCommand request, CancellationToken cancellationToken)
        {
            string warning = "Бот отключён";
            try
            {
                _bot.Stop();
            }
            catch(Exception e)
            {
                warning = e.Message;
            }
            return Task.FromResult(warning);
        }
    }
}
