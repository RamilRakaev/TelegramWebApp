using Infrastructure.CQRS.Commands.Requests.Telegram;
using MediatR;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotService;
using System;
using GoogleCalendarService;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Infrastructure.CQRS.Commands.Handlers.Telegram
{
    public class StartTelegramReceivingHandler : GetTelegramHandlers, IRequestHandler<StartTelegramReceivingCommand, string>
    {
        private readonly string[] appOptions = { "ApiKey", "CalandarId", "Token" };

        public StartTelegramReceivingHandler(
            IRepository<TelegramUser> usersRepository,
            IRepository<Option> optionRepository,
            IGoogleCalendar calendar,
            ILogger<AbstractTelegramHandlers> logger) : base(usersRepository, optionRepository, calendar, logger)
        {
        }

        public async Task<string> Handle(StartTelegramReceivingCommand request, CancellationToken cancellationToken)
        {
            var optionsFromDb = _optionRepository.GetAllAsNoTracking();
            var propertyNames = optionsFromDb.Select(o => o.PropertyName);
            var warning =  new StringBuilder("Не определены настройки: ");
            bool readiness = true;
            foreach (var appOption in appOptions)
            {
                var value = await propertyNames.ContainsAsync(appOption, cancellationToken: cancellationToken);
                if (value == false)
                {
                    readiness = false;
                    warning.Append($"{appOption} ");
                }
            }

            if (readiness)
            {
                try
                {
                    var bot = new TelegramBot(await optionsFromDb.ToArrayAsync(cancellationToken: cancellationToken), GetHandlers());
                    await bot.Start();
                    return "Телеграм бот запущен";
                }
                catch(Exception e)
                {
                    warning.Clear();
                    warning.Append(e.Message);
                }
            }
            return warning.ToString();
        }
    }
}
