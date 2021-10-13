using System.Threading.Tasks;
using TelegramWebApp.CQRSInfrastructure.Methods.Commands.Requests;
using TelegramWebApp.Pages.ClientPages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TelegramWebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IMediator _mediator;
        public readonly UserProperties _user;
        public UserLoginCommand Login { get; set; }

        public LoginModel(IMediator mediator,
            ILogger<LoginModel> logger,
            UserProperties userProperties)
        {
            Login = new UserLoginCommand();
            _mediator = mediator;
            _logger = logger;
            _user = userProperties;
        }

        public IActionResult OnGet()
        {
            _logger.LogInformation($"Login page visited");
            if (_user.IsAuthenticated)
                switch (_user.RoleId)
                {
                    case 1:
                        return RedirectToPage("/ClientPages/BookCatalog");
                    case 2:
                        return RedirectToPage("/AdminPages/UserList");
                    case 3:
                        return RedirectToPage("/LibrarianPages/BookCatalog");
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

            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                _logger.LogWarning($"Incorrect username and (or) password");
            }
            return RedirectToPage("/Account/Login");
        }
    }
}
