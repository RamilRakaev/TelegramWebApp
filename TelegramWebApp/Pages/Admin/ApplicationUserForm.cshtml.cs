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
    public class ApplicationUserFormModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserProperties _user;

        public ApplicationUserFormModel(IMediator mediator, UserProperties user)
        {
            _mediator = mediator;
            _user = user;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGet(int? userId)
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            if (userId != null)
            {
                ApplicationUser = await _mediator.Send(new GetUserQuery(userId.Value));
            }
            else
            {
                ApplicationUser = new ApplicationUser();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                applicationUser.RoleId = 1;
                await _mediator.Send(new CreateOrEditUserCommand(applicationUser));
                return RedirectToPage("ApplicationUsers");
            }
            ApplicationUser = new ApplicationUser();
            return Page();

        }
    }
}
