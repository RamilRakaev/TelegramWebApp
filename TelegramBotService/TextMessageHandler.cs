
namespace TelegramBotService
{
    public class TextMessageHandler
    {
        public TextMessageHandler()
        {

        }

        public TextMessageHandler(string command, string description, MessageHandlerReturningMessage handler)
        {
            Command = command;
            Description = description;
            Handler = handler;
        }

        public string Command { get; set; }
        public string Description { get; set; }
        public MessageHandlerReturningMessage Handler { get; set; }
    }
}
