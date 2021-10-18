using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace Infrastructure.CQRS.Commands.Validators.ApplicationUsers.User
{
    public class UserLoginValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty();
        }
    }
}
