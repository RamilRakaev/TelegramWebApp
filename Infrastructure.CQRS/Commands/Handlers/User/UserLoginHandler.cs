using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.CQRS.Commands.Request.User
{
    public class UserLoginHandler : IRequestHandler<UserLoginCommand, string>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<UserLoginHandler> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, false);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    await _userManager.AddClaimAsync(user, new Claim("roleId", user.RoleId.ToString()));
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("Succeeded login");
                    return "Вход успешно осуществлён";
                }
                else
                {
                    _logger.LogInformation("Password is not correct");
                    return "Неверный пароль";
                }

            }
            else
            {
                _logger.LogInformation("This postal address is not registered");
                return "Данный почтовый адрес не зарегистрирован";
            }
        }
    }
}

