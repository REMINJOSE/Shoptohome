using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shoptohome.Data;
using Shoptohome.Service;
using Shoptohome.Service.Interface;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
   options.UseSqlServer(builder.Configuration.GetConnectionString("Shoptohome"));
});

builder.Services.AddScoped<IHashPassword, HashPassword>();
builder.Services.AddScoped<IJWTService, JWTService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
      options.TokenValidationParameters = new TokenValidationParameters
      {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
         ValidateIssuer = false,
         ValidateAudience = false
      };
   });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
   options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
   {
      Description = "Standard Authorization header using the Bearer scheme (\"bearer  {token}\")",
      In = ParameterLocation.Header,
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey
   });
   options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
