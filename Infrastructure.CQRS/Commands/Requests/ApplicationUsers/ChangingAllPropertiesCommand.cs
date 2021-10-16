using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers
{
    public class ChangingAllPropertiesCommand : IRequest<Domain.Model.ApplicationUser>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
