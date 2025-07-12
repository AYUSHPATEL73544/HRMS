using Hrms.Api;
using Hrms.Core.Entities;
using Hrms.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

NLogBuilder.ConfigureNLog("nlog.config");

LogManager.Configuration.Variables["webRootPath"] = builder.Environment.WebRootPath;

builder.Host.UseNLog();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DataConnection"));
});

builder.Services.AddIdentity<User, Role<int>>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(x =>
  {
      x.RequireHttpsMetadata = false;
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters
      {
          ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
          ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Jwt:Secret"))),
          ValidateIssuer = true,
          ValidateAudience = false
      };
  });

builder.Services.AddDataProtection();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        config => config.WithOrigins(builder.Configuration.GetSection("AppSettings:ValidOrigins").Get<string[]>())
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.LoadAppSettings();

builder.Services.ConfigureServices();

builder.Services.ConfigureBackgroundHostedServices();

builder.Services.ConfigureRepositories();

builder.Services.ConfigureManagers();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.EnsureDbIsUpToDate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
