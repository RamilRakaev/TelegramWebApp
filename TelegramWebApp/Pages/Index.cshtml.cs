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

        public IndexModel(ILogger<IndexModel> logger, ILogger<ITelegramHandler> logger1, ITelegramHandler handler)
        {
            _logger = logger;

            TelegramBot telegram = new TelegramBot("2049358621:AAEvQjcOTzQ9OQsEc073xrfg6jbRtWKhX-o", handler);
            telegram.Start();
        }

        public void OnGet()
        {
        }
    }
}
