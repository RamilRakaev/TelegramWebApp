using Domain.Model;
using MediatR;

namespace  Infrastructure.CQRS.Commands.Request.User
{
    public class ChangePasswordCommand : IRequest<ApplicationUser>
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}