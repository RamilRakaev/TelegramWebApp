//using Domain.Model;
//using Infrastructure.Services.Mailing;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Threading.Tasks;

//namespace Infrastructure.CQRS.Commands.Request.User
//{
//    public static class Notification
//    {
//        public static async Task SendMessage(this PageModel page,
//            ApplicationUser user,
//            UserManager<ApplicationUser> userManager,
//            IMessageSending emailService)
//        {
//            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
//            var callbackUrl = page.Url.Page(pageName: "/Account/ConfirmEmail", pageHandler: "", values: new { userEmail = user.Email, code },
//                protocol: page.HttpContext.Request.Scheme);
//            await emailService.SendEmailAsync(user.Email, "Confirm your account",
//                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>LibraryAccounting</a>");
//        }
//    }
//}
