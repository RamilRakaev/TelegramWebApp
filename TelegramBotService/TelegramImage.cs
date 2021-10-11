using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotBusiness
{
    public class TelegramImage
    {
        public string Title { get; set; }
        public string Source { get; set; }
        public string Command { get; set; }

        public TelegramImage()
        {

        }

        public TelegramImage(string command, string source, string title)
        {
            Command = command;
            Source = source;
            Title = title;
        }
    }
}
