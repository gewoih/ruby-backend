using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Authority = builder.Configuration.GetValue<string>("PassportUrl");
		options.Audience = "casino-api";
		options.RequireHttpsMetadata = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = builder.Configuration["PassportUrl"],
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuer = true,
		};
	});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("ocelot.json", false, true)
	.AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);

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


await app.UseOcelot();

app.Run();
