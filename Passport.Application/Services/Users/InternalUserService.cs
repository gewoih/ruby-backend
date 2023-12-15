using Microsoft.AspNetCore.Identity;
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
				user = await GetByExternalIdAsync(externalUserId);
			}
			catch (KeyNotFoundException)
			{
				user = new InternalUser
				{
					ExternalId = externalUserId,
					AuthenticationMethod = authenticationMethod
				};

				var identityResult = await _userManager.CreateAsync(user);
			}

			return user;
		}

		public Task<InternalUser> GetByExternalIdAsync(string externalUserId)
		{
			throw new NotImplementedException();
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
