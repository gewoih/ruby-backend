using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
	[Route("login")]
	public class SteamAuthenticationController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			return Challenge("Steam");
		}

		[HttpGet("callback")]
		public async Task<IActionResult> LoginCallback()
		{
			var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
			return Ok();
		}
	}
}
