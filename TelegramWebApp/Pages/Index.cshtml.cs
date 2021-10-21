using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TelegramWebApp.Pages.Account;

namespace TelegramWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly UserProperties _user;

        public IndexModel(ILogger<IndexModel> logger, UserProperties user)
        {
            _logger = logger;
            _user = user;
        }

        public IActionResult OnGet()
        {
            _logger.LogInformation("Index page visited");
            return RedirectToPage("/Account/Login");
        }
    }
}
