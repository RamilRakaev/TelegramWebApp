using MediatR;
using Domain.Model;
using System.Collections.Generic;

namespace Infrastructure.CQRS.Commands.Request.Options
{
    public class EditTelegramOptionsRequest : IRequest<TelegramOptions>
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public List<TelegramUser> Users { get; set; }
    }
}
