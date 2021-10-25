using Infrastructure.CQRS.Commands.Requests.Telegram;
using MediatR;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotService;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Infrastructure.CQRS.Commands.Handlers.Telegram
{
    public class StartTelegramReceivingHandler : IRequestHandler<StartTelegramReceivingCommand, string>
    {
        private readonly string[] appOptions = { "ApiKey", "CalendarId", "Token" };
        private readonly ILogger<StartTelegramReceivingHandler> _logger;
        private readonly IRepository<Option> _optionRepository;
        private readonly AbstractTelegramBot _bot;

        public StartTelegramReceivingHandler(
            IRepository<Option> optionRepository,
            ILogger<StartTelegramReceivingHandler> logger,
            AbstractTelegramBot bot)
        {
            _optionRepository = optionRepository;
            _logger = logger;
            _bot = bot;
        }

        public async Task<string> Handle(StartTelegramReceivingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram Bot starting");
            var optionsFromDb = _optionRepository.GetAllAsNoTracking();
            var propertyNames = optionsFromDb.Select(o => o.PropertyName);
            var warning = new StringBuilder("Не определены настройки календаря: ");
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
                    await _bot.StartAsync((int)request.Mode);
                    return "Телеграм бот запущен";
                }
                catch (Exception e)
                {
                    warning.Clear();
                    warning.Append(e.Message);
                }
            }
            return warning.ToString();
        }
    }
}
