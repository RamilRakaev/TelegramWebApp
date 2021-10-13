
namespace Domain.Model
{
    public class TelegramUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int TelegramOptionsId { get; set; }
        public TelegramOptions TelegramOptions { get; set; }
    }
}
