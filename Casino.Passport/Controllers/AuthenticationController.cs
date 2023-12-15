using IdentityModel.Client;
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

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
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

            return Ok(tokenResponse.AccessToken);
		}
	}
}