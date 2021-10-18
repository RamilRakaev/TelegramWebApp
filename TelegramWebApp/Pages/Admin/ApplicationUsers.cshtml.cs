using System.Threading.Tasks;
using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using Infrastructure.CQRS.Queries.Request.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TelegramWebApp.Pages.Account;

namespace TelegramWebApp.Pages.Admin
{
    public class ApplicationUsersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserProperties _user;

        public ApplicationUsersModel(IMediator mediator, UserProperties user)
        {
            _mediator = mediator;
            _user = user;
        }

        public ApplicationUser[] ApplicationUsers { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            ApplicationUsers = await _mediator.Send(new GetUsersQuery());
            return Page();
        }

        public async Task<IActionResult> OnPost(int userId)
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            await _mediator.Send(new RemoveUserCommand(userId));
            ApplicationUsers = await _mediator.Send(new GetUsersQuery());
            return Page();
        }
    }
}
