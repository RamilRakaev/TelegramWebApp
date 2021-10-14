using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Commands.Request.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.Options
{
    public class EditTelegramOptionsHandler : IRequestHandler<EditTelegramOptionsRequest, TelegramOptions>
    {
        private readonly IRepository<TelegramOptions> _db;

        public EditTelegramOptionsHandler(IRepository<TelegramOptions> db)
        {
            _db = db;
        }

        public async Task<TelegramOptions> Handle(EditTelegramOptionsRequest request, CancellationToken cancellationToken)
        {
            var options = await _db.FindAsync(request.Id);
            options.Token = request.Token;
            options.Users = request.Users;
            await _db.SaveAsync();
            return options;
        }
    }
}
