using Casino.SharedLibrary.Services.MessageBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Services.Payments.Waxpeer;
using Wallet.Application.Services.Promocodes;
using Wallet.Infrastructure.Database;
using Wallet.Infrastructure.Services.NowPayments;
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

builder.Services.AddScoped<IMessageBusService, MessageBusService>();
builder.Services.AddScoped<IWaxpeerPaymentService, WaxpeerPaymentService>();
builder.Services.AddScoped<INowPaymentsApiService, NowPaymentsApiService>();
builder.Services.AddScoped<IWaxpeerApiService, WaxpeerService>();
builder.Services.AddScoped<IPromocodesService, PromocodesService>();

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
