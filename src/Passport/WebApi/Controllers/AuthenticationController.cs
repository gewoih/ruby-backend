using IdentityModel.Client;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
    public class SteamAuthenticationController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            });
        }

        [HttpGet("logout")]
        // [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties() { RedirectUri = "http://localhost:4200" }, "Cookies", "idsrv",
                "idsrv.external");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string scope, string session_state)
        {
            var tokenResponse = await new HttpClient().RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = "https://localhost:7220/connect/token",
                ClientId = "web_app",
                Code = code,
                CodeVerifier = "f822b246de6bd0d4fe2ffa0723da8a8627a3ccb05fb71b6018cee21a",
                GrantType = "authorization_code",
                RedirectUri = "https://localhost:7220/callback"
            });

            if (tokenResponse.IsError)
                return BadRequest(tokenResponse.Error);

            if (HttpContext.User.IsAuthenticated())
                return Ok($"Пользователь аутентифицирован: {tokenResponse.AccessToken}");
                
            return NotFound("Пользователь не прошел аутентификацию!");
        }
    }
}