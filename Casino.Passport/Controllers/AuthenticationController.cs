using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Passport.Application.Services.Users;
using Passport.Domain.Enums;

namespace Casino.Passport.Controllers
{
    [Route("login")]
    public class AuthenticationController : Controller
    {
	    private readonly IInternalUserService _userService;

	    public AuthenticationController(IInternalUserService userService)
	    {
		    _userService = userService;
	    }

		[HttpGet]
        public IActionResult Login()
        {
			return Challenge("Steam");
		}

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string? status)
        {
			var tokenResponse = await new HttpClient().RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
			{
				Address = "https://localhost:7220/connect/token",
				ClientId = "web_app",
				Code = code,
                CodeVerifier = "b9a8b29d9739e6ccfc6bcc41afa80d5b1376805afe66bb1f01a18946",
				GrantType = "authorization_code",
				RedirectUri = "https://localhost:7220/login/callback"
			});

			if (tokenResponse.IsError)
				return BadRequest(tokenResponse.Error);

			var steamId = HttpContext.User.Claims.First().Value;
			var currentIdentity = (ClaimsIdentity)HttpContext.User.Identity;
			currentIdentity.AddClaim(new Claim("sub", steamId));
			var result = await _userService.RegisterAsync(ExternalAuthenticationMethod.Steam, steamId);

			return Ok(tokenResponse.AccessToken);
		}
	}
}