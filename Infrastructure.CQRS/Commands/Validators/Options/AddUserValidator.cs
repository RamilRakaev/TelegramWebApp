using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.Options;

namespace Infrastructure.CQRS.Commands.Validators.Options
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserValidator()
        {
            RuleFor(u => u.User.UserName).NotNull().NotEmpty().MinimumLength(3);
        }
    }
}
