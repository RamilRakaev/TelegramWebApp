using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TelegramOptionsRepository : IRepository<TelegramOptions>
    {
        private readonly DataContext _db;

        public TelegramOptionsRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<TelegramOptions> FindNoTrackingAsync(int id)
        {
            return await _db
                .Set<TelegramOptions>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<TelegramOptions> FindAsync(int id)
        {
            return await _db
                .Set<TelegramOptions>()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<TelegramOptions> GetAll()
        {
            return _db
                .Set<TelegramOptions>()
                .AsQueryable();
        }

        public IQueryable<TelegramOptions> GetAllAsNoTracking()
        {
            return _db
                .Set<TelegramOptions>()
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task AddAsync(TelegramOptions element)
        {
            await _db.AddAsync(element);
        }

        public async Task RemoveAsync(TelegramOptions element)
        {
            await Task.Run(() => _db.Remove(element));
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
