using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Passport.Infrastructure.Database
{
	public sealed class PassportDbContext : IdentityDbContext
	{
		private readonly ConfigurationStoreOptions _configurationStoreOptions;
		private readonly OperationalStoreOptions _operationalStoreOptions;

		public PassportDbContext(
			DbContextOptions<PassportDbContext> options,
			ConfigurationStoreOptions configurationStoreOptions, 
			OperationalStoreOptions operationalStoreOptions) 
			: base(options)
		{
			_configurationStoreOptions = configurationStoreOptions;
			_operationalStoreOptions = operationalStoreOptions;
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.ConfigureClientContext(_configurationStoreOptions);
			builder.ConfigureResourcesContext(_configurationStoreOptions);
			builder.ConfigurePersistedGrantContext(_operationalStoreOptions);
		}
	}
}
