using Domain.Model;
using MediatR;

namespace Infrastructure.CQRS.Commands.Requests.Options
{
    public class AddUserCommand : IRequest<TelegramUser>
    {
        public AddUserCommand(TelegramUser user)
        {
            User = user;
        }

        public TelegramUser User { get; set; }
    }
}
