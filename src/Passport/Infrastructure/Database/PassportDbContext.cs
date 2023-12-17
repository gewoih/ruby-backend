using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure.Models;

namespace Passport.Infrastructure.Database
{
	public sealed class PassportDbContext : IdentityDbContext<InternalUser, InternalRole, Guid>
	{
		public PassportDbContext(DbContextOptions<PassportDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
