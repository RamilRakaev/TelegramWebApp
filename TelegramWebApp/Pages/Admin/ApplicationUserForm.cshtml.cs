using System.Threading.Tasks;
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

        public CreateOrEditUserCommand ApplicationUser { get; set; }

        public async Task<IActionResult> OnGet(int? userId)
        {
            if (_user.RoleId != 1)
            {
                return RedirectToPage("/Account/Login");
            }
            if (userId != null)
            {
                var user = await _mediator.Send(new GetUserQuery(userId.Value));
                ApplicationUser = new CreateOrEditUserCommand(user.Email, "", user.Id);
            }
            else
            {
                ApplicationUser = new CreateOrEditUserCommand();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(CreateOrEditUserCommand applicationUser)
        {
            if (ModelState.IsValid)
            {
                applicationUser.RoleId = 1;
                var result = await _mediator.Send(applicationUser);
                if (result.Succeeded)
                    return RedirectToPage("ApplicationUsers");
            }
            ApplicationUser = new CreateOrEditUserCommand();
            return Page();

        }
    }
}
