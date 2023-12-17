using System.Reflection;
using System.Security.Claims;
using Casino.Passport.Config;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Passport.Application.Services.Users;
using Passport.Infrastructure.Database;
using Passport.Infrastructure.Models;
using Passport.WebApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.FullName;

//builder.Services.AddScoped<IInternalUserService, InternalUserService>();

//builder.Services.AddIdentity<InternalUser, InternalRole>()
//	.AddEntityFrameworkStores<PassportDbContext>()
//	.AddDefaultTokenProviders();

//builder.Services.AddDbContext<PassportDbContext>(options => 
//	options.UseNpgsql(connectionString, 
//		sql => sql.MigrationsAssembly(migrationsAssembly)));

//SeedData.EnsureSeedData(connectionString);

builder.Services
	.AddIdentityServer(options =>
	{
		options.UserInteraction.ErrorUrl = "/error";
		options.UserInteraction.LoginUrl = "/login";
		options.UserInteraction.LogoutUrl = "/logout";
	})
	//.AddAspNetIdentity<InternalUser>()
	//.AddConfigurationStore(options =>
	//{
	//	options.ConfigureDbContext = b =>
	//		b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
	//})
	.AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
	.AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
	.AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
	.AddInMemoryClients(IdentityServerConfig.GetClients())
	.AddOperationalStore(options =>
	{
		options.ConfigureDbContext = b =>
			b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
	})
	//.AddProfileService<SteamProfileService>()
	.AddDeveloperSigningCredential();

builder.Services
	.AddAuthentication()
	.AddCookie()
	.AddSteam(options =>
	{
		options.Events.OnTicketReceived = context =>
		{
			var steamId = context.Principal.Claims.First().Value;
			var currentIdentity = (ClaimsIdentity)context.Principal.Identity;
			currentIdentity.AddClaim(new Claim("sub", steamId));

			return Task.CompletedTask;
		};
	});

builder.Services.AddControllers();
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
await scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.MigrateAsync();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();