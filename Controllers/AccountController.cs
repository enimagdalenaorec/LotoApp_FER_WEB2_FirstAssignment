using Auth0.AspNetCore.Authentication;
using LotoApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("LoginCallback", "Account", new { returnUrl }))
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> LoginCallback(string returnUrl = "/")
        {
            var sub = User.FindFirst("sub")?.Value
              ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
              ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var email = User.FindFirst("email")?.Value
                        ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
                        ?? User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                        ?? User.Identity?.Name;

            Console.WriteLine($"Sub: {sub}, Email: {email}");

            if (!string.IsNullOrEmpty(sub))
            {
                await _authService.GetOrCreateUserAsync(sub, email);
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
