using System.Threading.Tasks;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TelegramWebApp.Pages.Account;

namespace TelegramWebApp.Pages.Admin
{
    public class OptionsFormModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserProperties _user;

        public OptionsFormModel(IMediator mediator, UserProperties user)
        {
            _mediator = mediator;
            _user = user;
        }

        public IActionResult OnGet()
        {
            if(_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(WebAppOptions appOptions)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new ChangeOptionsCommand(appOptions));
                return RedirectToPage("AdminPanel");
            }
            else
            {
                ModelState.AddModelError("", "Не должно быть пустых полей");
                return Page();
            }
        }
    }
}
