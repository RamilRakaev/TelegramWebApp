using Domain.Model;
using MediatR;

namespace  Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class ChangePasswordCommand : IRequest<Domain.Model.ApplicationUser>
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }
}