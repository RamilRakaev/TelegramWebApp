
namespace TelegramBotService
{
    public class TextMessageHandler
    {
        public TextMessageHandler()
        {

        }

        public TextMessageHandler(string command, string description, TelegramBotService.MessageHandlerReturningMessage handler)
        {
            Command = command;
            Description = description;
            Handler = handler;
        }

        public string Command { get; set; }
        public string Description { get; set; }
        public TelegramBotService.MessageHandlerReturningMessage Handler { get; set; }
    }
}
