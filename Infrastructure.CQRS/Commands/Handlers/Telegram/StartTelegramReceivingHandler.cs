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

namespace Infrastructure.CQRS.Commands.Handlers.Telegram
{
    public class StartTelegramReceivingHandler : GetTelegramHandlers, IRequestHandler<StartTelegramReceivingCommand, string>
    {
        public StartTelegramReceivingHandler(
            IRepository<TelegramUser> usersRepository,
            IRepository<Option> optionRepository,
            IGoogleCalendar calendar,
            ILogger<AbstractTelegramHandlers> logger) : base(usersRepository, optionRepository, calendar, logger)
        {
        }

        public async Task<string> Handle(StartTelegramReceivingCommand request, CancellationToken cancellationToken)
        {
            WebAppOptions webAppOptions = new WebAppOptions();
            var options = _optionRepository.GetAllAsNoTracking();
            var propertyNames = options.Select(o => o.PropertyName);
            string warning = "Не определены настройки: ";
            bool readiness = true;
            foreach (var property in webAppOptions.GetType().GetProperties())
            {
                var value = await propertyNames.ContainsAsync(property.Name);
                if (value == false)
                {
                    readiness = false;
                    warning += $"{property.Name} ";
                }
            }

            if (readiness)
            {
                try
                {
                    var bot = new TelegramBot(await options.ToArrayAsync(), GetHandlers());
                    await bot.Start();
                    return "Телеграм бот запущен";
                }
                catch(Exception e)
                {
                    warning = e.Message;
                }
            }
            return warning;
        }
    }
}
