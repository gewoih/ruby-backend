using Microsoft.AspNetCore.Identity;
using Passport.Domain.Enums;

namespace Passport.Infrastructure.Models
{
	public sealed class InternalUser : IdentityUser<Guid>
	{
		public ExternalAuthenticationMethod AuthenticationMethod { get; set; }
		public string ExternalId { get; set; }
		public string SteamTradeLink { get; set; }
	}
}
