using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TelegramWebApp.Pages.Account;
using TelegramBotService;

namespace TelegramWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, AbstractTelegramBot telegramBot)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            _logger.LogInformation("Index page visited");
            return RedirectToPage("/Account/Login");
        }
    }
}
