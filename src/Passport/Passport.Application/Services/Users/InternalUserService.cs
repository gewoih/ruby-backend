using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Passport.Domain.Enums;
using Passport.Domain.Models;
using Passport.Infrastructure.Database;

namespace Passport.Application.Services.Users
{
	public sealed class InternalUserService : IInternalUserService
	{
		private readonly PassportDbContext _context;
		private readonly UserManager<InternalUser> _userManager;

		public InternalUserService(PassportDbContext context, UserManager<InternalUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<InternalUser> RegisterAsync(ExternalAuthenticationMethod authenticationMethod, string externalUserId)
		{
			var user = await GetByExternalIdAsync(authenticationMethod, externalUserId);
			if (user is null)
			{
				user = new InternalUser
				{
					UserName = externalUserId,
					ExternalId = externalUserId,
					AuthenticationMethod = authenticationMethod
				};

				var identityResult = await _userManager.CreateAsync(user);
				if (!identityResult.Succeeded)
					throw new Exception(identityResult.ToString());
			}

			return user;
		}

		public async Task<InternalUser?> GetByExternalIdAsync(ExternalAuthenticationMethod authenticationMethod, string externalUserId)
		{
			var foundUser = await _context.Users.FirstOrDefaultAsync(user =>
				user.AuthenticationMethod == authenticationMethod && 
				user.ExternalId == externalUserId);

			return foundUser;
		}

		public async Task<InternalUser?> GetByIdAsync(Guid id)
		{
			var foundUser = await _context.Users.FindAsync(id);
			return foundUser;
		}

		public async Task<bool> PatchUserAsync(InternalUser userToPatch, JsonPatchDocument<InternalUser> patchDocument)
		{
			patchDocument.ApplyTo(userToPatch);
			var updatedRows = await _context.SaveChangesAsync();

			return updatedRows > 0;
		}
	}
}
