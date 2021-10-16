using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using Infrastructure.CQRS.Commands.Requests.Telegram;
using Infrastructure.CQRS.Queries.Request.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TelegramWebApp.Pages.Admin
{
    public class AdminPanelModel : PageModel
    {
        private readonly IMediator _mediator;

        public AdminPanelModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public TelegramUser[] Users { get; set; }
        public string Warning { get; set; } = string.Empty;

        public async Task OnGet()
        {
            Users = await _mediator.Send(new GetAllTelegramUsersQuery());
        }

        public async Task OnPost(int userId)
        {
            await _mediator.Send(new RemoveUserCommand(userId));
            Users = await _mediator.Send(new GetAllTelegramUsersQuery());
        }

        public async Task OnGetTurn()
        {
            Warning = await _mediator.Send(new StartTelegramReceivingCommand());
            Users = await _mediator.Send(new GetAllTelegramUsersQuery());
        }

        public async Task OnGetTurnOff()
        {
            Warning = await _mediator.Send(new StopTelegramReceivingCommand());
            Users = await _mediator.Send(new GetAllTelegramUsersQuery());
        }
    }
}
