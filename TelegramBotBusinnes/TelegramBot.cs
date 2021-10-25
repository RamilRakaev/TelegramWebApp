using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public enum Mode : int { Updates, Webhook }
    public enum BotStatus { OnInUpdatesMode, OnInWebhookMode, Off }

    public class TelegramBot : AbstractTelegramBot
    {
        public TelegramBot(AbstractTelegramHandlers handlers, IRepository<Option> repository) :
            base(handlers, repository.GetAllAsNoTracking().ToArray())
        {
        }
    }
}
