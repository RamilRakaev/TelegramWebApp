using System.Collections.Generic;

namespace TelegramBotBusiness
{
    public class TelegramInlineButton
    {
        public TelegramInlineButton()
        {

        }

        public TelegramInlineButton(string command, List<CallbackQueryMessageHandler> callbacks)
        {
            Command = command;
            Callbacks = callbacks;
        }

        public string Command { get; set; }
        public List<CallbackQueryMessageHandler> Callbacks { get; set; }
    }
}
