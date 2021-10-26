using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace Infrastructure.CQRS.Commands.Validators.ApplicationUsers
{
    public class ChangingAllUserPropertiesValidator : AbstractValidator<ChangingAllPropertiesCommand>
    {
        public ChangingAllUserPropertiesValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.RoleId).NotEmpty();
            RuleFor(c => c.Password).NotEmpty().MinimumLength(10);
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
        }
    }
}
