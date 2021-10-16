using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.Options
{
    public class RemoveUserCommand : IRequest<TelegramUser>
    {
        public RemoveUserCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
