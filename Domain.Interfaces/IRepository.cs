using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<Element> : IStorageRequests<Element>
    {
        public Task AddAsync(Element element);

        public Task RemoveAsync(Element element);

        public Task RemoveRangeAsync(IEnumerable<Element> elements)
        {
            throw new Exception("Method is not overridden in child class");
        }

        public Task SaveAsync();
    }
}
