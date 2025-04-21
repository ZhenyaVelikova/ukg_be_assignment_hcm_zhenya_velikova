using HCM.BusinessLogic;
using HCM.BusinessLogic.Models.Authorization;
using HCM.Core.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile($"appsettings.json", true, true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, true);

var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
builder.Services.ConfigureBusinessLogic(connectionStrings);

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();



var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = "Bearer";
	options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtOptions.Issuer,
		ValidAudience = jwtOptions.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
		ClockSkew = TimeSpan.Zero,
		
	};
	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = context =>
		{
			Console.WriteLine("Authentication failed: " + context.Exception.Message);
			return Task.CompletedTask;
		},
		OnTokenValidated = context =>
		{
			Console.WriteLine("Token validated successfully.");
			return Task.CompletedTask;
		}
	};
	;
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors(builder => builder
	.AllowAnyHeader()
	.AllowAnyOrigin()
	.AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
