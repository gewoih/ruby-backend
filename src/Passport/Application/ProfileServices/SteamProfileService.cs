using System.Security.Claims;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Passport.Application.Services.Users;
using Passport.Domain.Enums;

namespace Passport.Application.ProfileServices
{
	public sealed class SteamProfileService : IProfileService
	{
		private readonly IInternalUserService _userService;

		public SteamProfileService(IInternalUserService userService)
		{
			_userService = userService;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var steamId = context.Subject.GetSubjectId().Split("/").Last();
			var user = await _userService.RegisterAsync(ExternalAuthenticationMethod.Steam, steamId);

			var userIdClaim = new Claim("id", user.Id.ToString());
			var steamIdClaim = new Claim("steam_id", steamId);
			
			context.IssuedClaims.AddRange([userIdClaim, steamIdClaim]);
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			context.IsActive = true;
		}
	}
}
