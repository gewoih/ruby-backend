using IdentityModel.Client;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Passport.WebApi.Controllers
{
	public class SteamAuthenticationController(IIdentityServerInteractionService interaction) : Controller
	{
		private readonly IIdentityServerInteractionService _interaction = interaction;

		[HttpGet("login")]
		public IActionResult Login(string returnUrl)
		{
			return Challenge(new AuthenticationProperties
			{
				RedirectUri = returnUrl
			});
		}

		[HttpGet("logout")]
		// [ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(string logoutId)
		{
			var logout = await _interaction.GetLogoutContextAsync(logoutId);

			if (User.Identity?.IsAuthenticated == true)
			{
				await HttpContext.SignOutAsync();
			}

			return Redirect(logout?.PostLogoutRedirectUri ?? "~/");
		}

		[HttpGet("callback")]
		public async Task<IActionResult> Callback(string code, string scope, string session_state)
		{
			var tokenResponse = await new HttpClient().RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
			{
				Address = "https://localhost:7220/connect/token",
				ClientId = "web_app",
				Code = code,
				CodeVerifier = "f822b246de6bd0d4fe2ffa0723da8a8627a3ccb05fb71b6018cee21a",
				GrantType = "authorization_code",
				RedirectUri = "https://localhost:7220/callback"
			});

			if (tokenResponse.IsError)
				return BadRequest(tokenResponse.Error);

			if (HttpContext.User.IsAuthenticated())
				return Ok($"Пользователь аутентифицирован: {tokenResponse.AccessToken}");

			return NotFound("Пользователь не прошел аутентификацию!");
		}
	}
}