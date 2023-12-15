using System.Reflection;
using System.Security.Claims;
using Casino.Passport.Config;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure.Database;
using Passport.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var identityConfig = new IdentityServerConfig(builder.Configuration);

builder.Services.AddDbContext<PassportDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<PassportDbContext>()
	.AddDefaultTokenProviders();

builder.Services
	.AddIdentityServer(opt =>
	{
		opt.UserInteraction.ErrorUrl = "/error";
		opt.UserInteraction.LoginUrl = "/login";
		opt.UserInteraction.LogoutUrl = "/logout";
	})
	.AddDeveloperSigningCredential()
	.AddAspNetIdentity<User>()
	.AddInMemoryApiScopes(identityConfig.GetApiScopes())
	.AddInMemoryApiResources(identityConfig.GetApiResources())
	.AddInMemoryIdentityResources(identityConfig.GetIdentityResources())
	.AddInMemoryClients(identityConfig.GetClients())
	.AddConfigurationStore(options =>
	{
		options.ConfigureDbContext = optionsBuilder =>
			optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
	})
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

await using var scope = app.Services.CreateAsyncScope();
var passportDbContext = scope.ServiceProvider.GetRequiredService<PassportDbContext>();
passportDbContext.Database.Migrate();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();