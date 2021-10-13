
namespace Domain.Model
{
    public class TelegramOptions
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public TelegramUser[] Users { get; set; }
    }
}
