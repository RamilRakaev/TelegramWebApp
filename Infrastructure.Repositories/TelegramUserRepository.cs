using Domain.Model;

namespace Infrastructure.Repositories
{
    public class TelegramUserRepository : BaseRepository<TelegramUser>
    {
        public TelegramUserRepository(DataContext context) : base( context)
        { }
    }
}
