using System.Threading.Tasks;
using TelegramWebApp.CQRSInfrastructure.Methods.Commands.Requests.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TelegramWebApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMediator _mediator;
        public UserRegistrationCommand Register { get; set; }

        public RegisterModel(
            ILogger<RegisterModel> logger,
            IMediator mediator)
        {
            Register = new UserRegistrationCommand();
            _logger = logger;
            _mediator = mediator;
        }

        public void OnGet()
        {
            _logger.LogInformation($"Register page visited");
        }

        public async Task<IActionResult> OnPost(UserRegistrationCommand register)
        {
            if (ModelState.IsValid)
            {
                register.Page = this;
                var message = await _mediator.Send(register);
                ModelState.AddModelError(string.Empty, message);
            }
            return Page();
        }
    }
}
