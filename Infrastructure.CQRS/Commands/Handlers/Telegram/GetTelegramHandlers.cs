using Domain.Interfaces;
using Domain.Model;
using GoogleCalendarService;
using Microsoft.Extensions.Logging;
using System.Linq;
using TelegramBotBusiness;
using TelegramBotService;

namespace Infrastructure.CQRS.Commands.Handlers.Telegram
{
    public class GetTelegramHandlers
    {
        protected readonly IRepository<TelegramUser> _usersRep;
        protected readonly IRepository<Option> _optionRep;
        private readonly IGoogleCalendar _calendar;
        private readonly ILogger<AbstractTelegramHandlers> _logger;

        public GetTelegramHandlers(
            IRepository<TelegramUser> usersRep,
            IRepository<Option> optionRep,
            IGoogleCalendar calendar,
            ILogger<AbstractTelegramHandlers> logger)
        {
            _usersRep = usersRep;
            _optionRep = optionRep;
            _calendar = calendar;
            _logger = logger;
        }

        public AbstractTelegramHandlers GetHandlers()
        {
            var users = _usersRep.GetAllAsNoTracking().ToArray();
            var config = new HandlerConfiguration(_calendar);
            return new TelegramHandlers(_optionRep.GetAllAsNoTracking().ToArray(), users, _logger, config);
        }
    }
}
