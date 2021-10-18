using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    public class GetWebAppOptionsHandler : IRequestHandler<GetWebAppOptionsQuery, WebAppOptions>
    {
        private readonly IRepository<Option> _db;

        public GetWebAppOptionsHandler(IRepository<Option> db)
        {
            _db = db;
        }

        public Task<WebAppOptions> Handle(GetWebAppOptionsQuery request, CancellationToken cancellationToken)
        {
            WebAppOptions webAppOptions = new WebAppOptions();
            var options = _db.GetAllAsNoTracking();
            foreach (var property in webAppOptions.GetType().GetProperties())
            {
                var option = options.FirstOrDefault(o => o.PropertyName == property.Name) ?? new Option() { Value = "Значение не введено"};
                property.SetValue(webAppOptions, option.Value);
            }
            return Task.FromResult(webAppOptions);
        }
    }
}
