using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBotBusiness;

namespace TelegramWebApp.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(
            [FromBody] Update update)
        {
            await TelegramBot.EchoAsync(update);
            return Ok();
        }
    }
}
