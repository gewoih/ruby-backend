using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Casino.WebApi.Controllers
{
	[Authorize]
	[Route("home")]
	public class HomeController : Controller
	{
		[HttpGet("test")]
		public IActionResult Test()
		{
			return Ok("piska");
		}
	}
}
