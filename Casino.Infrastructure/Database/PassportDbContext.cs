using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure.Models;

namespace Passport.Infrastructure.Database
{
	public sealed class PassportDbContext : IdentityDbContext<InternalUser, InternalRole, Guid>
	{
		private readonly OperationalStoreOptions _operationalStoreOptions;

		public PassportDbContext(
			DbContextOptions<PassportDbContext> options, 
			OperationalStoreOptions operationalStoreOptions) 
			: base(options)
		{
			_operationalStoreOptions = operationalStoreOptions;
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ConfigurePersistedGrantContext(_operationalStoreOptions);
		}
	}
}
