using Microsoft.AspNetCore.Mvc;
using Passport.Application.Services.Users;
using Passport.Domain.Enums;

namespace Casino.Passport.Controllers
{
	public class TestController : Controller
	{
		private readonly IInternalUserService _userService;

		public TestController(IInternalUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(string externalUserId)
		{
			var user = await _userService.RegisterAsync(ExternalAuthenticationMethod.Steam, externalUserId);
			return Json(user);
		}
	}
}
