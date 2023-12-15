using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
    [Route("login")]
    public class AuthenticationController : Controller
    {
		[HttpGet]
		public IActionResult Login()
		{
			return Challenge("Steam");
		}
	}
}