using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    public class GetSettingsReadinessHandler : IRequestHandler<GetSettingsReadinessQuery, bool>
    {
        private readonly IRepository<Option> _db;

        public GetSettingsReadinessHandler(IRepository<Option> db)
        {
            _db = db;
        }

        public async Task<bool> Handle(GetSettingsReadinessQuery request, CancellationToken cancellationToken)
        {
            WebAppOptions webAppOptions = new WebAppOptions();
            var options = _db.GetAllAsNoTracking();
            foreach (var property in webAppOptions.GetType().GetProperties())
            {
                var value = await options.FirstOrDefaultAsync(o => o.PropertyName == property.Name) != null;
                if (value == false)
                    return false;
            }
            return true;
        }
    }
}
