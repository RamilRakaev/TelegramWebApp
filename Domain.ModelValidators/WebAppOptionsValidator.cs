﻿using Domain.Model;
using FluentValidation;

namespace Domain.ModelValidators
{
    public class WebAppOptionsValidator : AbstractValidator<WebAppOptions>
    {
        public WebAppOptionsValidator()
        {
            RuleFor(o => o.ApiKey).NotNull().NotEmpty();
            RuleFor(o => o.CalendarId).NotNull().NotEmpty();
            RuleFor(o => o.Token).NotNull().NotEmpty();
        }
    }
}
