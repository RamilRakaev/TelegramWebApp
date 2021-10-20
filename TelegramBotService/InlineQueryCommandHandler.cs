
namespace TelegramBotService
{
    public class InlineQueryCommandHandler
    {
        public InlineQueryCommandHandler()
        {

        }

        public InlineQueryCommandHandler(string command, InlineQueryHandler handler)
        {
            Command = command;
            Handler = handler;
        }

        public string Command { get; set; }
        public InlineQueryHandler Handler { get; set; }
    }
}
