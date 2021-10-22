
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

        public InlineQueryCommandHandler(string command, string description, InlineQueryHandler handler)
        {
            Command = command;
            Description = description;
            Handler = handler;
        }

        public string Command { get; set; }
        public string Description { get; set; }
        public InlineQueryHandler Handler { get; set; }
    }
}
