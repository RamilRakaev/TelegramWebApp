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
using TelegramWebApp.Pages.Account;

namespace TelegramWebApp.Pages.Admin
{
    public class AdminPanelModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserProperties _user;

        public AdminPanelModel(IMediator mediator, UserProperties user)
        {
            _mediator = mediator;
            _user = user;
        }

        public TelegramUser[] Users { get; set; }
        public string Warning { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet()
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Index");
            }
            Users = await _mediator.Send(new GetAllTelegramUsersQuery());
            return Page();
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
