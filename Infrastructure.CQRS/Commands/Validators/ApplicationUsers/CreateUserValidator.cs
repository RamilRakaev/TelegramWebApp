using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace Infrastructure.CQRS.Commands.Validators.ApplicationUsers.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.User.Id).Equal(0);

            RuleFor(u => u.User.UserName)
                .NotNull()
                .NotEmpty()
                .Length(3, 50);

            RuleFor(u => u.User.Email)
                .EmailAddress();

            RuleFor(u => u.User.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$")
                .WithMessage("Неправильно введён пароль");
        }
    }
}
