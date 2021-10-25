using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class TelegramBot : BaseTelegramBot
    {
        public TelegramBot(AbstractTelegramHandlers handlers, IRepository<Option> repository) :
            base(handlers, repository.GetAllAsNoTracking().ToArray())
        {
        }
    }
}
