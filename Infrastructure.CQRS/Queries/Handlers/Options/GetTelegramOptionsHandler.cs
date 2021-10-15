using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    class GetTelegramOptionsHandler : IRequestHandler<GetTelegramOptionsQuery, TelegramOptions>
    {
        private readonly IRepository<TelegramOptions> _db;

        public GetTelegramOptionsHandler(IRepository<TelegramOptions> db)
        {
            _db = db;
        }

        public Task<TelegramOptions> Handle(GetTelegramOptionsQuery request, CancellationToken cancellationToken)
        {
            var options = _db.GetAllAsNoTracking().First();
            return Task.FromResult(options);
        }
    }
}
