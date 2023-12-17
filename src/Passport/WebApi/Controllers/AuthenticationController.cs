using IdentityModel.Client;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Passport.Controllers
{
	[Route("login")]
	public class SteamAuthenticationController : Controller
	{
		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return Challenge(new AuthenticationProperties
			{
				RedirectUri = returnUrl
			}, "Steam");
		}

		[HttpGet("callback")]
		public async Task<IActionResult> Callback(string code, string scope, string session_state)
		{
			return Ok();
			//var tokenResponse = await new HttpClient().RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
			//{
			//	Address = "https://localhost:7220/connect/token",
			//	ClientId = "web_app",
			//	Code = code,
			//	CodeVerifier = "f822b246de6bd0d4fe2ffa0723da8a8627a3ccb05fb71b6018cee21a",
			//	GrantType = "authorization_code",
			//	ClientSecret = "A43DCC44-AC8C-46EE-B312-ECA6F6CBFA52",
			//	RedirectUri = "https://localhost:7220/login/callback"
			//});

			//if (tokenResponse.IsError)
			//	return BadRequest(tokenResponse.Error);

			//if (HttpContext.User.IsAuthenticated())
			//	return Ok($"Пользователь аутентифицирован: {tokenResponse.AccessToken}");
			//else
			//	return NotFound("Пользователь не прошел аутентификацию!");
		}
	}
}