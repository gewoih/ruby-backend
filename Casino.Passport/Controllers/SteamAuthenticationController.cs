using System.Security.Claims;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
    [Route("login")]
    public class SteamAuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/test" }, "Steam");
        }

        //TODO: потом удалить)))
        //Оставил пока что чтоб было, но оно не нужно ни для чего
        // [HttpGet("callback")]
        // public async Task<IActionResult> LoginCallback()
        // {
        //     var result = await HttpContext.AuthenticateAsync("Steam");
        //
        //     if (result.Succeeded != true) return Ok();
        //
        //     var claimsPrincipal = new ClaimsPrincipal(
        //         new ClaimsIdentity(
        //             new[]
        //             {
        //                 new Claim(ClaimTypes.NameIdentifier,
        //                     result.Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)
        //             },
        //             BearerTokenDefaults.AuthenticationScheme
        //         )
        //     );
        //
        //     return SignIn(claimsPrincipal);
        // }
    }
}