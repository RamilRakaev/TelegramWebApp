using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.Options
{
    class RemoveUserHandler : IRequestHandler<RemoveUserCommand, TelegramUser>
    {
        private readonly IRepository<TelegramUser> _db;

        public RemoveUserHandler(IRepository<TelegramUser> db)
        {
            _db = db;
        }

        public async Task<TelegramUser> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.FindAsync(request.Id);
            await _db.RemoveAsync(user);
            await _db.SaveAsync();
            return user;
        }
    }
}
