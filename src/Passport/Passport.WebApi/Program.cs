using System.Reflection;
using System.Security.Claims;
using AspNet.Security.OpenId.Steam;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Passport.Application.ProfileServices;
using Passport.Application.Services.Users;
using Passport.Domain.Models;
using Passport.Infrastructure.Database;
using Passport.WebApi.Config;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.FullName;

builder.Services.AddScoped<IInternalUserService, InternalUserService>();

builder.Services.AddIdentity<InternalUser, InternalRole>()
	.AddEntityFrameworkStores<PassportDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddDbContext<PassportDbContext>(options =>
	options.UseNpgsql(connectionString,
		sql => sql.MigrationsAssembly(migrationsAssembly)));

builder.Services
	.AddIdentityServer(options =>
	{
		options.Events.RaiseErrorEvents = true;
		options.Events.RaiseInformationEvents = true;
		options.Events.RaiseFailureEvents = true;
		options.Events.RaiseSuccessEvents = true;

		options.EmitStaticAudienceClaim = true;

		options.UserInteraction.ErrorUrl = "/error";
		options.UserInteraction.LoginUrl = "/login";
		options.UserInteraction.LogoutUrl = "/logout";
	})
	.AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
	.AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
	.AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
	.AddInMemoryClients(IdentityServerConfig.GetClients())
	.AddAspNetIdentity<InternalUser>()
	.AddProfileService<SteamProfileService>()
	.AddOperationalStore(options =>
	{
		options.ConfigureDbContext = b =>
			b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
	})
	.AddDeveloperSigningCredential();

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = IdentityConstants.ExternalScheme;
		options.DefaultChallengeScheme = SteamAuthenticationDefaults.AuthenticationScheme;
	})
	.AddCookie()
	.AddSteam(options =>
	{
		options.Events.OnTicketReceived = async context =>
		{
			var steamLink = context.Principal.Claims.First().Value;
			var currentIdentity = (ClaimsIdentity)context.Principal.Identity;
			currentIdentity.AddClaim(new Claim("sub", steamLink));
			currentIdentity.AddClaim(new Claim("idp", context.Scheme.Name));

			await context.HttpContext.SignInAsync(IdentityConstants.ExternalScheme, context.Principal);
		};
	});

builder.Services.AddControllers()
	.AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
await scope.ServiceProvider.GetService<PassportDbContext>().Database.MigrateAsync();
await scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.MigrateAsync();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();