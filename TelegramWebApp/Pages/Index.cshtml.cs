using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotService;

namespace TelegramWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ILogger<ITelegramHandler> logger1, TelegramBot telegram)
        {
            _logger = logger;

            TelegramBot _telegram = telegram;
            _telegram.Start();
        }

        public void OnGet()
        {
        }
    }
}
