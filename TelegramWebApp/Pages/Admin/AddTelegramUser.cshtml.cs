using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Model;
using MediatR;
using Infrastructure.CQRS.Commands.Requests.Options;
using TelegramWebApp.Pages.Account;

namespace TelegramWebApp.Pages.Admin
{
    public class AddTelegramUserModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserProperties _user;

        public AddTelegramUserModel(IMediator mediator, UserProperties user)
        {
            _mediator = mediator;
            _user = user;
        }

        public IActionResult OnGet()
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/");
            }
            return Page();
        }

        public async Task OnPost(TelegramUser user)
        {
            await _mediator.Send(new AddUserCommand(user));
        }
    }
}
