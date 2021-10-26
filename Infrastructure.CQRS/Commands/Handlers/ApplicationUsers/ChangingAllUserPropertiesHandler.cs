using Domain.Model;
using Infrastructure.CQRS.Commands.Requests.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Handlers.ApplicationUsers
{
    public class ChangingAllUserPropertiesHandler : UserHandler, IRequestHandler<ChangingAllPropertiesCommand, ApplicationUser>
    {
        public ChangingAllUserPropertiesHandler(UserManager<ApplicationUser> db, ILogger<ChangingAllUserPropertiesHandler> logger) : base(db, logger)
        { }

        public async Task<ApplicationUser> Handle(ChangingAllPropertiesCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken: cancellationToken);
            user.UserName = command.Email;
            user.Email = command.Email;
            user.Password = command.Password;
            user.RoleId = command.RoleId;
            await _userManager.UpdateAsync(user);
            _logger.LogDebug($"User: {user.Email}");
            _logger.LogInformation("Changing all user properties");
            return user;
        }
    }
}
