using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.CQRS.Commands.Requests.ApplicationUsers.User
{
    //public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, string>
    //{
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly ILogger<UserRegistrationHandler> _logger;
    //    private readonly IMessageSending _emailService;

    //    public UserRegistrationHandler(ILogger<UserRegistrationHandler> logger,
    //        UserManager<ApplicationUser> userManager,
    //        IMessageSending emailService)
    //    {
    //        _userManager = userManager;
    //        _logger = logger;
    //        _emailService = emailService;
    //    }

    //    public async Task<string> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    //    {
    //        var existingUser = await _userManager.FindByEmailAsync(request.Email);
    //        if (existingUser != null)
    //        {
    //            return await CheckingExistingAccount(request, existingUser);
    //        }
    //        else
    //        {
    //            return await CreateAccount(request);
    //        }
    //    }

    //    private async Task<string> CreateAccount(UserRegistrationCommand request)
    //    {
    //        var user = new ApplicationUser()
    //        {
    //            UserName = request.UserName,
    //            Email = request.Email,
    //            Password = request.Password,
    //            RoleId = request.RoleId
    //        };
    //        var result = await _userManager.CreateAsync(user, request.Password);
    //        if (result.Succeeded)
    //        {
    //            _logger.LogInformation($"Sending message for user");
    //            await request.Page.SendMessage(user, _userManager, _emailService);
    //            return "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме";
    //        }
    //        else
    //        {
    //            string errorMessages = "";
    //            foreach (var error in result.Errors)
    //            {
    //                _logger.LogError($"{error.Description}");
    //                errorMessages += error.Description + "\n";
    //            }
    //            return errorMessages;
    //        }
    //    }

    //    private async Task<string> CheckingExistingAccount(UserRegistrationCommand request, ApplicationUser existingUser)
    //    {
    //        if (await _userManager.IsEmailConfirmedAsync(existingUser) == false)
    //        {
    //            _logger.LogInformation($"This account already exists, mail is not verified");
    //            await request.Page.SendMessage(existingUser, _userManager, _emailService);
    //            return "Аккаунт с текущей почтой уже существует. Почта ещё не подтверждена. " +
    //                "Чтобы подтвердить email перейдите по ссылке в письме";
    //        }
    //        else
    //        {
    //            _logger.LogInformation($"This account already exists");
    //            return "Аккаунт с текущей почтой уже существует";
    //        }
    //    }
    //}
}
