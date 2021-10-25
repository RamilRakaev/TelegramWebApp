using Domain.Interfaces;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.Options
{
    public class ChangeOptionsHandler : IRequestHandler<ChangeOptionsCommand, Option[]>
    {
        private readonly IRepository<Option> _db;

        public ChangeOptionsHandler(IRepository<Option> db)
        {
            _db = db;
        }

        public async Task<Option[]> Handle(ChangeOptionsCommand request, CancellationToken cancellationToken)
        {
            var options = _db.GetAll();
            foreach (var property in request.Options.Properties.Keys)
            {
                var option = await options.FirstOrDefaultAsync(o => o.PropertyName == property, cancellationToken: cancellationToken);
                if (option != null)
                {
                    option.Value = request.Options[property];
                }
                else
                {
                    await _db.AddAsync(new Option(property, request.Options[property]));
                }
            }
            await _db.SaveAsync();
            return options.ToArray();
        }
    }
}
