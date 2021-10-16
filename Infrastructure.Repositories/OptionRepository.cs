using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class OptionRepository : IRepository<Option>
    {
        private readonly DataContext _db;

        public OptionRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Option> FindNoTrackingAsync(int id)
        {
            return await _db
                .Set<Option>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Option> FindAsync(int id)
        {
            return await _db
                .Set<Option>()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<Option> GetAll()
        {
            return _db
                .Set<Option>()
                .AsQueryable();
        }

        public IQueryable<Option> GetAllAsNoTracking()
        {
            return _db
                .Set<Option>()
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task AddAsync(Option element)
        {
            await _db.AddAsync(element);
        }

        public async Task RemoveAsync(Option element)
        {
            await Task.Run(() => _db.Remove(element));
        }

        public async Task RemoveRangeAsync(IEnumerable<Option> elements)
        {
            await Task.Run(() => _db.Set<Option>().RemoveRange(elements));
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
