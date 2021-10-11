using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotService
{
    public class TelegramOptions
    {
        public const string position = "TelegramOptions";
        public string Token { get; set; }
        public string[] Users { get; set; }
    }
}
