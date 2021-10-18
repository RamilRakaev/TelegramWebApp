using Domain.Model;
using FluentValidation;

namespace Domain.ModelValidators
{
    public class ApplicationUserValidator : AbstractValidator<ApplicationUser>
    {
        public ApplicationUserValidator()
        {
            RuleFor(u => u.UserName)
                .NotNull()
                .NotEmpty();

            RuleFor(u => u.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .Length(10, 50)
                .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$");
        }
    }
}
