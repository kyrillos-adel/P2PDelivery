using System.Text;
using AutoMapper;
using P2PDelivery.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using P2PDelivery.API.Middelwares;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Services;
using P2PDelivery.Infrastructure;
using P2PDelivery.Infrastructure.Configurations;
using P2PDelivery.Infrastructure.Contexts;
using P2PDelivery.Application.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(DeliveryRequestProfile));

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDeliveryRequestService, DeliveryRequestService>();
builder.Services.AddScoped<IAuthService, AuthService>();


JwtSettings.Initialize(builder.Configuration);

var key = Encoding.UTF8.GetBytes(JwtSettings.SecretKey);
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = JwtSettings.Issuer,
        ValidateIssuer = true,

        ValidAudience = JwtSettings.Audience,
        ValidateAudience = true,

        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,

        ValidateLifetime = true
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
