using Domain.Interfaces;
using Domain.Model;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class BaseRepository<Element> : IRepository<Element> where Element :BaseEntity  
    {
        private readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Element> FindNoTrackingAsync(int id)
        {
            return await _context
                .Set<Element>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Element> FindAsync(int id)
        {
            return await _context
                .Set<Element>()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<Element> GetAll()
        {
            return _context
                .Set<Element>()
                .AsQueryable();
        }

        public IQueryable<Element> GetAllAsNoTracking()
        {
            return _context
                .Set<Element>()
                .AsQueryable()
                .AsNoTracking();
        }

        public async Task AddAsync(Element element)
        {
            await _context.AddAsync(element);
        }

        public async Task RemoveAsync(Element element)
        {
            await Task.Run(() => _context.Remove(element));
        }

        public Task RemoveRangeAsync(IEnumerable<Element> elements)
        {
            _context.Set<Element>().RemoveRange(elements);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
