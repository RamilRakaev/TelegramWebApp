using System.Threading.Tasks;
using TelegramWebApp.CQRSInfrastructure.Methods.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TelegramWebApp.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly ILogger<ConfirmEmailModel> _logger;
        private readonly IMediator _mediator;

        public ConfirmEmailModel(
            ILogger<ConfirmEmailModel> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGet(string userEmail, string code)
        {
            _logger.LogInformation($"ConfirmEmail page visited");
            var user = await _mediator.Send(new ConfirmEmailCommand(userEmail, code));
            switch (user.RoleId)
            {
                case 1:
                    return RedirectToPage("/ClientPages/BookCatalog");
                case 2:
                    return RedirectToPage("/LibrarianPages/BookCatalog");
                case 3:
                    return RedirectToPage("/AdminPages/UserList");
                default:
                    break;
            }
            return RedirectToPage("/Account/Login");
        }
    }
}
