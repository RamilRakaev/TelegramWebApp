using TelegramBotService;

namespace TelegramBotService
{
    public class CallbackQueryMessageHandler
    {
        public CallbackQueryMessageHandler()
        {

        }

        public CallbackQueryMessageHandler(string command, CallbackQueryHandler handler)
        {
            Command = command;
            Handler = handler;
        }

        public string Command { get; set; }
        public CallbackQueryHandler Handler { get; set; }
    }
}
