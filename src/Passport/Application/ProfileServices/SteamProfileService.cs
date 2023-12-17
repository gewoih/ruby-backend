using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Passport.Application.Services.Users;
using Passport.Domain.Enums;
using Passport.Infrastructure.Models;

namespace Passport.Application.ProfileServices
{
	public sealed class SteamProfileService : ProfileService<InternalUser>
	{
		private readonly IInternalUserService _userService;

		public SteamProfileService(IInternalUserService userService, 
			UserManager<InternalUser> userManager, 
			IUserClaimsPrincipalFactory<InternalUser> claimsFactory) : base(userManager, claimsFactory)
		{
			_userService = userService;
		}

		public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var steamId = context.Subject.GetSubjectId();
			var user = await _userService.RegisterAsync(ExternalAuthenticationMethod.Steam, steamId);

			var claims = new List<Claim>
			{
				new("Id", user.Id.ToString())
			};

			context.AddRequestedClaims(claims);
		}
	}
}
