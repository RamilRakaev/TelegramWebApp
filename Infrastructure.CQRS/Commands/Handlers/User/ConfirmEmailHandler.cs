//using Domain.Model;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Infrastructure.CQRS.Commands.Request.User
//{
//    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, ApplicationUser>
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly ILogger<ConfirmEmailHandler> _logger;
//        private readonly SignInManager<ApplicationUser> _signInManager;

//        public ConfirmEmailHandler(UserManager<ApplicationUser> userManager,
//            ILogger<ConfirmEmailHandler> logger,
//            SignInManager<ApplicationUser> signInManager)
//        {
//            _userManager = userManager;
//            _logger = logger;
//            _signInManager = signInManager;
//        }

//        public async Task<ApplicationUser> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation($"ConfirmEmail page visited");
//            if (request.Email == null || request.Code == null)
//            {
//                _logger.LogError($"Arguments userEmail and code are zero");
//            }
//            var user = await _userManager.FindByEmailAsync(request.Email);
//            if (user == null)
//            {
//                user = new ApplicationUser();
//                _logger.LogError($"User is not found");
//            }
//            var mailConfirmationResult = await _userManager.ConfirmEmailAsync(user, request.Code);
//            if (mailConfirmationResult.Succeeded)
//            {
//                _logger.LogInformation($"User confirmation was successful");
//                var signInResult = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
//                if (signInResult.Succeeded)
//                {
//                    await _signInManager.SignInAsync(user, false);
//                    await _userManager.AddClaimAsync(user, new Claim("roleId", user.RoleId.ToString()));
//                    await _userManager.UpdateAsync(user);
//                    _logger.LogInformation($"Succeeded login");
//                }
//                return user;
//            }
//            return user;
//        }
//    }
//}
