using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Model;
using MediatR;
using Infrastructure.CQRS.Commands.Requests.Options;

namespace TelegramWebApp.Pages.Admin
{
    public class AddTelegramUserModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddTelegramUserModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnGet()
        { }

        public async Task OnPost(TelegramUser user)
        {
            await _mediator.Send(new AddUserCommand(user));
        }
    }
}
