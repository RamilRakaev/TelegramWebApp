using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Queries.Handlers.Options
{
    public class GetAllTelegramUsersHandler : IRequestHandler<GetAllTelegramUsersQuery, TelegramUser[]>
    {
        private readonly IRepository<TelegramUser> _db;

        public GetAllTelegramUsersHandler(IRepository<TelegramUser> db)
        {
            _db = db;
        }

        public async Task<TelegramUser[]> Handle(GetAllTelegramUsersQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => _db.GetAllAsNoTracking().ToArray());
        }
    }
}
