using Domain.Model;
using FluentValidation;

namespace Domain.ModelValidators
{
    public class TelegramUserValidator : AbstractValidator<TelegramUser>
    {
        public TelegramUserValidator()
        {
            RuleFor(u => u.UserName)
                .NotNull()
                .NotEmpty()
                .Length(3, 50);
        }
    }
}
