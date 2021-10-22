using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class TelegramBot : AbstractTelegramBot
    {
        public TelegramBot(IRepository<Option> optionRepository, AbstractTelegramHandlers handlers) : 
            base(optionRepository.GetAllAsNoTracking().ToArray(), handlers)
        {
        }
    }
}
