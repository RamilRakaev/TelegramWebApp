using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace TelegramWebApp.Pages.Account
{
    public class UserProperties
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserProperties(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int UserId
        {
            get
            {
                var result = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (result != null)
                {
                    return Convert.ToInt32(result.Value);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int RoleId
        {
            get
            {
                var result = _httpContext.HttpContext.User.FindFirst("roleId"); 
                if (result != null)
                {
                    return Convert.ToInt32(result.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string RoleName { get
            {
                var result = _httpContext.HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
                if(result != null)
                {
                    return result.Value;
                }
                else
                {
                    return string.Empty;
                }
            } }

        public bool IsAuthenticated
        {
            get { return _httpContext.HttpContext.User.Identity.IsAuthenticated; }
        }
    }
}
