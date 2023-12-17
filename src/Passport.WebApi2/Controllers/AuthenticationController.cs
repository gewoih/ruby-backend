using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
    [Route("login")]
    public class AuthenticationController : Controller
    {
		[HttpGet]
		public IActionResult Login()
		{
			return Challenge(new AuthenticationProperties
			{
				RedirectUri = "/login/test"
			}, "Steam");
		}

		[HttpGet("test")]
		public IActionResult Test()
		{
			return Ok(HttpContext.User.IsAuthenticated());
		}

		[HttpGet("callback")]
		public IActionResult Callback(string code, string state)
		{
			return Ok();
		}
	}
}