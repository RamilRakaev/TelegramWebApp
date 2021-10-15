using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Commands.Request.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.Options
{
    public class ChangeCalendarOptionsHandler : IRequestHandler<ChangeCalendarOptionsCommand, CalendarOptions>
    {
        private readonly IRepository<CalendarOptions> _db;

        public ChangeCalendarOptionsHandler(IRepository<CalendarOptions> db)
        {
            _db = db;
        }

        public async Task<CalendarOptions> Handle(ChangeCalendarOptionsCommand request, CancellationToken cancellationToken)
        {
            var options = await _db.FindAsync(request.Options.Id);
            options.Name = request.Options.Name;
            options.ApiKey = request.Options.ApiKey;
            options.CalandarId = request.Options.CalandarId;
            await _db.SaveAsync();
            return options;
        }
    }
}
