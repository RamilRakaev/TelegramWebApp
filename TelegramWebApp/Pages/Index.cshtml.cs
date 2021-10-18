using GoogleCalendarService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotService;
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
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }
    }
}
