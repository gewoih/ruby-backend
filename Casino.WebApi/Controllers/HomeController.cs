using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Casino.WebApi.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public IActionResult Test()
		{
			return Ok("piska");
		}
	}
}
