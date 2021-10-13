using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class TelegramUserRepository : IRepository<TelegramUser>
    {
        private readonly DataContext _db;

        public TelegramUserRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<TelegramUser> FindNoTrackingAsync(int id)
        {
            return await _db
                .Set<TelegramUser>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<TelegramUser> FindAsync(int id)
        {
            return await _db
                .Set<TelegramUser>()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<TelegramUser> GetAll()
        {
            return _db
                .Set<TelegramUser>()
                .AsQueryable();
        }

        public IQueryable<TelegramUser> GetAllAsNoTracking()
        {
            return _db
                .Set<TelegramUser>()
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task AddAsync(TelegramUser element)
        {
            await _db.AddAsync(element);
        }

        public async Task RemoveAsync(TelegramUser element)
        {
            await Task.Run(() => _db.Remove(element));
        }

        public async Task RemoveRangeAsync(IEnumerable<TelegramUser> elements)
        {
            await Task.Run(() => _db.Set<TelegramUser>().RemoveRange(elements));
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
