using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.Options
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, TelegramUser>
    {
        private readonly IRepository<TelegramUser> _db;

        public AddUserHandler(IRepository<TelegramUser> db)
        {
            _db = db;
        }

        public async Task<TelegramUser> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _db.AddAsync(request.User);
            await _db.SaveAsync();
            return request.User;
        }
    }
}
