using GoogleCalendarService;
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
        private readonly IGoogleCalendar _google;
        private readonly TelegramBot _telegram;

        public IndexModel(ILogger<IndexModel> logger, ILogger<ITelegramHandler> logger1, TelegramBot telegram, IGoogleCalendar google)
        {
            _logger = logger;
            _google = google;
            _telegram = telegram;
        }

        public async Task OnGet()
        {
            var events = await _google.ShowUpCommingEvents();
            //_telegram.Start();
        }
    }
}
