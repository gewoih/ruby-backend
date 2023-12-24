using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Passport.Application.Services.Users;
using Passport.Domain.Models;

namespace Passport.WebApi.Controllers
{
	[Route("api/users")]
	public class UsersController : Controller
	{
		private readonly IInternalUserService _userService;

		public UsersController(IInternalUserService userService)
		{
			_userService = userService;
		}

		[HttpPatch("update")]
		public async Task<IActionResult> Update(Guid userId, [FromBody] JsonPatchDocument<InternalUser>? patchDocument)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			
			var foundUser = await _userService.GetByIdAsync(userId);
			if (foundUser is null)
				return NotFound();

			_ = await _userService.PatchUserAsync(foundUser, patchDocument);
			return Ok();
		}
	}
}
