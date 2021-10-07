﻿using Microsoft.AspNetCore.Mvc;
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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //"2099065574:AAH8g3Ja370fcWjagaV-K_4QvHHX2COLYFY"
            TelegramBot telegram = new TelegramBot("2049358621:AAEvQjcOTzQ9OQsEc073xrfg6jbRtWKhX-o");
            telegram.Start();
        }
    }
}
