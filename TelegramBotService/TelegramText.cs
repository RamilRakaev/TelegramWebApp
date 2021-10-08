using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotService
{
    public class TelegramText
    {
        public TelegramText()
        {

        }

        public TelegramText(string command, string description, string text)
        {
            Command = command;
            Description = description;
            Text = text;
        }

        public string Text { get; set; }
        public string Description { get; set; }
        public string Command { get; set; }
    }
}
