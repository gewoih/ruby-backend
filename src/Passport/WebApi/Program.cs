using System.Reflection;
using Casino.Passport.Config;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Passport.Application.ProfileServices;
using Passport.Application.Services.Users;
using Passport.Infrastructure.Database;
using Passport.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var identityConfig = new IdentityServerConfig(builder.Configuration);

builder.Services.AddDbContext<PassportDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IInternalUserService, InternalUserService>();

builder.Services.AddIdentity<InternalUser, InternalRole>()
	.AddEntityFrameworkStores<PassportDbContext>()
	.AddDefaultTokenProviders();

builder.Services
	.AddIdentityServer(opt =>
	{
		opt.UserInteraction.LoginUrl = "/login";
	})
	.AddProfileService<SteamProfileService>()
	.AddDeveloperSigningCredential()
	.AddAspNetIdentity<InternalUser>()
	.AddInMemoryApiScopes(identityConfig.GetApiScopes())
	.AddInMemoryApiResources(identityConfig.GetApiResources())
	.AddInMemoryIdentityResources(identityConfig.GetIdentityResources())
	.AddInMemoryClients(identityConfig.GetClients())
	.AddOperationalStore(options =>
	{
		options.ConfigureDbContext = optionsBuilder =>
			optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
		options.EnableTokenCleanup = true;
	});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
    })
    .AddCookie("Cookies")
    .AddSteam(options =>
    {
	    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
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

await using var scope = app.Services.CreateAsyncScope();
var passportDbContext = scope.ServiceProvider.GetRequiredService<PassportDbContext>();
passportDbContext.Database.Migrate();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();