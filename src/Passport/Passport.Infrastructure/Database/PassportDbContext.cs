using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Passport.Domain.Models;

namespace Passport.Infrastructure.Database
{
	public sealed class PassportDbContext : IdentityDbContext<InternalUser, InternalRole, Guid>
	{
		public PassportDbContext(DbContextOptions<PassportDbContext> options) : base(options)
		{
		}
	}
}
