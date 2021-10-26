using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;

namespace TelegramWebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IMediator _mediator;
        public readonly UserProperties _user;

        public LoginModel(IMediator mediator,
            ILogger<LoginModel> logger,
            UserProperties userProperties)
        {
            Login = new UserLoginCommand();
            _mediator = mediator;
            _logger = logger;
            _user = userProperties;
        }

        public UserLoginCommand Login { get; set; }

        public IActionResult OnGet()
        {
            _logger.LogInformation($"Login page visited");
            if (_user.IsAuthenticated)
                switch (_user.RoleId)
                {
                    case 1:
                        return RedirectToPage("/Admin/AdminPanel");
                }
            return Page();
        }

        public async Task<IActionResult> OnPost(UserLoginCommand login)
        {
            if (ModelState.IsValid)
            {
                login.Page = this;
                string message = await _mediator.Send(login);
                ModelState.AddModelError(string.Empty, message);
                return RedirectToPage("/Account/Login");
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                _logger.LogWarning($"Incorrect username and (or) password");
                return Page();
            }
        }
    }
}
