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
			var steamId = context.Subject.GetSubjectId();
			var user = await _userService.RegisterAsync(ExternalAuthenticationMethod.Steam, steamId);

			var claim = new Claim("Id", user.Id.ToString());

			context.IssuedClaims.Add(claim);
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var userId = context.Subject.GetSubjectId();
			try
			{
				var user = await _userService.GetByExternalIdAsync(ExternalAuthenticationMethod.Steam, userId);
				context.IsActive = true;
			}
			catch (KeyNotFoundException)
			{
				context.IsActive = false;
			}
		}
	}
}
