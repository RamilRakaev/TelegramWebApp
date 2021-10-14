using System.Collections.Generic;

namespace Domain.Model
{
    public class TelegramOptions
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public List<TelegramUser> Users { get; set; }
    }
}
