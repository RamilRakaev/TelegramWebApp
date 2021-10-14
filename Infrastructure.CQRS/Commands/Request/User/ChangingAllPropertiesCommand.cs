using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Request.User
{
    public class ChangingAllPropertiesCommand : IRequest<ApplicationUser>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
