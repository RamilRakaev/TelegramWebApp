using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Infrastructure.CQRS.Commands.Request.User
{
    public class UserLoginCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public PageModel Page { get; set; }
    }
}
