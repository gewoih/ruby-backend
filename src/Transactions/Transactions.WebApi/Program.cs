using MassTransit;
using Microsoft.EntityFrameworkCore;
using Transactions.Application.Consumers;
using Transactions.Application.Services.Transactions;
using Transactions.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<TransactionsDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IBalanceTransactionsService, BalanceTransactionsService>();

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<TransactionsTriggersConsumer>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var database = scope.ServiceProvider.GetService<TransactionsDbContext>()?.Database;
await database.MigrateAsync();

app.Run();
