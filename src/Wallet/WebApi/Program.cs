using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Services.Wallet;
using Wallet.Infrastructure;
using Wallet.Infrastructure.Database;
using Wallet.Infrastructure.Services.Waxpeer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<WalletDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWaxpeerService, WaxpeerService>();

builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, configuration) =>
    {
		configuration.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        configuration.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var database = scope.ServiceProvider.GetService<WalletDbContext>()?.Database;
await database.MigrateAsync();

app.Run();
