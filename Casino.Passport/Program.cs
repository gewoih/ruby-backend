using System.Reflection;
using System.Security.Claims;
using Casino.Passport.Config;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PassportDb");
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var identityConfig = new IdentityServerConfig(builder.Configuration);

builder.Services
    .AddIdentityServer()
    .AddInMemoryApiScopes(identityConfig.GetApiScopes())
    .AddInMemoryApiResources(identityConfig.GetApiResources())
    .AddInMemoryIdentityResources(identityConfig.GetIdentityResources())
    .AddInMemoryClients(identityConfig.GetClients())
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder =>
            optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
        options.EnableTokenCleanup = true;
    })
    .AddDeveloperSigningCredential();;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options => { options.LoginPath = "/login"; })
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

using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
await serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/test", (ClaimsPrincipal user) => user.Claims.First().Value)
    .RequireAuthorization();

app.Run();