using System.Reflection;
using Casino.Passport.Config;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;
using Passport.Application.ProfileServices;
using Passport.Application.Services.Users;
using Passport.Infrastructure.Database;
using Passport.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

//builder.Services.AddDbContext<PassportDbContext>(options => options.UseNpgsql(connectionString));
//builder.Services.AddScoped<IInternalUserService, InternalUserService>();

//builder.Services.AddIdentity<InternalUser, InternalRole>()
//	.AddEntityFrameworkStores<PassportDbContext>();

builder.Services
	.AddIdentityServer(options =>
	{
		options.UserInteraction.LoginUrl = "/login";
	})
	.AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
	.AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
	.AddInMemoryClients(IdentityServerConfig.GetClients(builder.Configuration))
	//.AddAspNetIdentity<InternalUser>()
	//.AddProfileService<SteamProfileService>()
	.AddOperationalStore(options =>
	{
		options.ConfigureDbContext = optionsBuilder =>
			optionsBuilder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
		options.EnableTokenCleanup = true;
	});

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	})
	.AddCookie(options =>
	{
		options.LoginPath = "/login";
	})
	.AddSteam(options =>
	{
		options.ApplicationKey = "2DD20C5634C5FE20A800E55A4CBB9C61";
		options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await using var scope = app.Services.CreateAsyncScope();
//var passportDbContext = scope.ServiceProvider.GetRequiredService<PassportDbContext>();
var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
//await passportDbContext.Database.MigrateAsync();
await persistedGrantDbContext.Database.MigrateAsync();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
