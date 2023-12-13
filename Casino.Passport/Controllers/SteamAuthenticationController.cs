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
			var result = Challenge(new AuthenticationProperties { RedirectUri = "/test" }, "Steam");

			return result;
		}
	}
}