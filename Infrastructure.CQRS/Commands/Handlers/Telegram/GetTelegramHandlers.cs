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
        protected readonly IRepository<TelegramUser> _usersRepository;
        protected readonly IRepository<Option> _optionRepository;
        private readonly ILogger<AbstractTelegramHandlers> _logger;

        public GetTelegramHandlers(
            IRepository<TelegramUser> usersRep,
            IRepository<Option> optionRep,
            ILogger<AbstractTelegramHandlers> logger)
        {
            _usersRepository = usersRep;
            _optionRepository = optionRep;
            _logger = logger;
        }

        public AbstractTelegramHandlers GetHandlers()
        {
            var users = _usersRepository.GetAllAsNoTracking().ToArray();
            var config = new HandlerConfiguration(_optionRepository);
            return new TelegramHandlers(_optionRepository.GetAllAsNoTracking().ToArray(), users, _logger, config);
        }
    }
}
