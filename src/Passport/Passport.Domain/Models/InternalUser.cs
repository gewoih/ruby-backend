using Microsoft.AspNetCore.Identity;
using Passport.Domain.Enums;

namespace Passport.Domain.Models
{
	public sealed class InternalUser : IdentityUser<Guid>
	{
		public ExternalAuthenticationMethod AuthenticationMethod { get; set; }
		public string ExternalId { get; set; }
		public string SteamTradeLink { get; set; }
	}
}
