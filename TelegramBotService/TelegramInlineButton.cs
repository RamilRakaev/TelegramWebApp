using System.Collections.Generic;

namespace TelegramBotService
{
    public class TelegramInlineButton
    {
        public TelegramInlineButton()
        {

        }

        public TelegramInlineButton(string command, List<TelegramCallbackQuery> callbacks)
        {
            Command = command;
            Callbacks = callbacks;
        }

        public string Command { get; set; }
        public List<TelegramCallbackQuery> Callbacks { get; set; }
    }
}
