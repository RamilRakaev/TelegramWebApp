using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Request.Options
{
    public class ChangeCalendarOptionsCommand : IRequest<CalendarOptions>
    {
        public ChangeCalendarOptionsCommand(CalendarOptions options)
        {
            Options = options;
        }

        public CalendarOptions Options { get; set; }
    }
}
