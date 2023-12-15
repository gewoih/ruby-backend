using Passport.Domain.Enums;
using Passport.Infrastructure.Models;

namespace Passport.Application.Services.Users
{
	public interface IInternalUserService
	{
		Task<InternalUser> RegisterAsync(ExternalAuthenticationMethod authenticationMethod, string externalUserId);
		Task<InternalUser> GetByExternalIdAsync(string externalUserId);
		Task<InternalUser> GetByIdAsync(Guid id);
	}
}
