using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CalendarOptionsRepository : IRepository<CalendarOptions>
    {
        private readonly DataContext _db;

        public CalendarOptionsRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<CalendarOptions> FindNoTrackingAsync(int id)
        {
            return await _db
                .Set<CalendarOptions>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<CalendarOptions> FindAsync(int id)
        {
            return await _db
                .Set<CalendarOptions>()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<CalendarOptions> GetAll()
        {
            return _db
                .Set<CalendarOptions>()
                .AsQueryable();
        }

        public IQueryable<CalendarOptions> GetAllAsNoTracking()
        {
            return _db
                .Set<CalendarOptions>()
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task AddAsync(CalendarOptions element)
        {
            await _db.AddAsync(element);
        }

        public async Task RemoveAsync(CalendarOptions element)
        {
            await Task.Run(() => _db.Remove(element));
        }

        public async Task RemoveRangeAsync(IEnumerable<CalendarOptions> elements)
        {
            await Task.Run(() => _db.Set<CalendarOptions>().RemoveRange(elements));
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
