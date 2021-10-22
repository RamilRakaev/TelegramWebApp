using Domain.Model;
using FluentValidation;

namespace Domain.ModelValidators
{
    public class CalendarOptionsValidator : AbstractValidator<WebAppOptions>
    {
        public CalendarOptionsValidator()
        {
            RuleFor(o => o.ApiKey).NotNull().NotEmpty();
            RuleFor(o => o.CalendarId).NotNull().NotEmpty();
        }
    }
}
