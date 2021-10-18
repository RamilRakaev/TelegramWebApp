using FluentValidation;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace Infrastructure.CQRS.Commands.Validators.ApplicationUsers
{
    public class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
    {
        public RemoveUserValidator()
        {
            RuleFor(b => b.Id).NotNull().NotEqual(0);
        }
    }
}
