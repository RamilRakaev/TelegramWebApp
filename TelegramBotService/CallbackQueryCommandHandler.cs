
namespace TelegramBotService
{
    public class CallbackQueryCommandHandler
    {
        public CallbackQueryCommandHandler()
        {

        }

        public CallbackQueryCommandHandler(string command, CallbackQueryHandler handler)
        {
            Command = command;
            Handler = handler;
        }

        public string Command { get; set; }
        public CallbackQueryHandler Handler { get; set; }
    }
}
