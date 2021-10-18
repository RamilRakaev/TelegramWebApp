using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.Options;

namespace Infrastructure.CQRS.Commands.Validators.Options
{
    public class ChangeOptionsValidator : AbstractValidator<ChangeOptionsCommand>
    {
        public ChangeOptionsValidator()
        {
            RuleFor(o => o.Options.ApiKey).NotNull().NotEmpty();
            RuleFor(o => o.Options.CalandarId).NotNull().NotEmpty();
            RuleFor(o => o.Options.Token).NotNull().NotEmpty();
        }
    }
}
