using System.Security.Claims;
using Casino.Passport.Config;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure.Database;

namespace Passport.WebApi
{
	public class SeedData
	{
		public static void EnsureSeedData(string connectionString)
		{
			var services = new ServiceCollection();
			services.AddLogging();
			
			services.AddOperationalDbContext(
				options =>
				{
					options.ConfigureDbContext = db =>
						db.UseNpgsql(connectionString, 
							sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
				}
			);
			services.AddConfigurationDbContext(
				options =>
				{
					options.ConfigureDbContext = db =>
						db.UseNpgsql(connectionString,
							sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
				}
			);

			var serviceProvider = services.BuildServiceProvider();

			using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

			var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
			context.Database.Migrate();

			EnsureSeedData(context);

			var ctx = scope.ServiceProvider.GetService<PassportDbContext>();
			ctx.Database.Migrate();
			EnsureUsers(scope);
		}

		private static void EnsureUsers(IServiceScope scope)
		{
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

			var angella = userManager.FindByNameAsync("angella").Result;
			if (angella == null)
			{
				angella = new IdentityUser
				{
					UserName = "angella",
					Email = "angella.freeman@email.com",
					EmailConfirmed = true
				};
				var result = userManager.CreateAsync(angella, "123").Result;
				if (!result.Succeeded)
					throw new Exception(result.Errors.First().Description);

				result = userManager.AddClaimsAsync(
						angella,
						new Claim[]
						{
							new(JwtClaimTypes.Name, "Angella Freeman"),
							new(JwtClaimTypes.GivenName, "Angella"),
							new(JwtClaimTypes.FamilyName, "Freeman"),
							new(JwtClaimTypes.WebSite, "http://angellafreeman.com"),
							new("location", "somewhere")
						}
					).Result;

				if (!result.Succeeded)
					throw new Exception(result.Errors.First().Description);
			}
		}

		private static void EnsureSeedData(ConfigurationDbContext context)
		{
			if (!context.Clients.Any())
			{
				foreach (var client in IdentityServerConfig.GetClients().ToList())
				{
					context.Clients.Add(client.ToEntity());
				}

				context.SaveChanges();
			}

			if (!context.IdentityResources.Any())
			{
				foreach (var resource in IdentityServerConfig.GetIdentityResources().ToList())
				{
					context.IdentityResources.Add(resource.ToEntity());
				}

				context.SaveChanges();
			}

			if (!context.ApiScopes.Any())
			{
				foreach (var resource in IdentityServerConfig.GetApiScopes().ToList())
				{
					context.ApiScopes.Add(resource.ToEntity());
				}

				context.SaveChanges();
			}

			if (!context.ApiResources.Any())
			{
				foreach (var resource in IdentityServerConfig.GetApiResources().ToList())
				{
					context.ApiResources.Add(resource.ToEntity());
				}

				context.SaveChanges();
			}
		}
	}
}