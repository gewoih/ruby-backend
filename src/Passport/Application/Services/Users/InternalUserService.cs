using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Passport.Domain.Enums;
using Passport.Infrastructure.Database;
using Passport.Infrastructure.Models;

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
			InternalUser user;
			try
			{
				user = await GetByExternalIdAsync(authenticationMethod, externalUserId);
			}
			catch (KeyNotFoundException)
			{
				user = new InternalUser
				{
					UserName = externalUserId,
					ExternalId = externalUserId,
					AuthenticationMethod = authenticationMethod
				};

				var identityResult = await _userManager.CreateAsync(user);
			}

			return user;
		}

		public async Task<InternalUser> GetByExternalIdAsync(ExternalAuthenticationMethod authenticationMethod, string externalUserId)
		{
			var foundUser = await _context.Users.FirstOrDefaultAsync(user =>
				user.AuthenticationMethod == authenticationMethod && 
				user.ExternalId == externalUserId);

			if (foundUser is null)
				throw new KeyNotFoundException($"Пользователь с ExternalID '{externalUserId}' во внешнем сервисе '{authenticationMethod}' не найден.");

			return foundUser;
		}

		public async Task<InternalUser> GetByIdAsync(Guid id)
		{
			var foundUser = await _context.Users.FindAsync(id);
			if (foundUser is null)
				throw new KeyNotFoundException($"Пользователь с ID '{id}' не найден.");

			return foundUser;
		}
	}
}
