
namespace TelegramBotService
{
    public class TelegramCallbackQuery
    {
        public TelegramCallbackQuery()
        {

        }

        public TelegramCallbackQuery(string command, string caption, string answer)
        {
            Command = command;
            Caption = caption;
            Answer = answer;
        }

        public string Command { get; set; }
        public string Caption { get; set; }
        public string Answer { get; set; }
    }
}
