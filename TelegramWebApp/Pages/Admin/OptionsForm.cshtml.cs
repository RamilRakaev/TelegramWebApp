using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TelegramWebApp.Pages.Admin
{
    public class OptionsFormModel : PageModel
    {
        private readonly IMediator _mediator;

        public OptionsFormModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnGet()
        { }

        public async Task OnPost(WebAppOptions webAppOptions)
        {
            await _mediator.Send(new ChangeOptionsCommand(webAppOptions));
        }
    }
}
