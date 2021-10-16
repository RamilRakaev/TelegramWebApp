using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    public class GetOptionHandler : IRequestHandler<GetOptionQuery, Option>
    {
        private readonly IRepository<Option> _db;

        public GetOptionHandler(IRepository<Option> db)
        {
            _db = db;
        }

        public async Task<Option> Handle(GetOptionQuery request, CancellationToken cancellationToken)
        {
            var option = await _db
                .GetAllAsNoTracking()
                .DefaultIfEmpty(new Option())
                .FirstOrDefaultAsync(o => o.PropertyName == request.PropertyName);
            return option;
        }
    }
}
