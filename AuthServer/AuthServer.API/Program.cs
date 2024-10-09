using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repositories;
using AuthServer.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), SqlOptions =>
    {
        SqlOptions.MigrationsAssembly("AuthServer.Data");
    });
});

builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

var tokenOptions = (builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>())!;

builder.addCustomTokenAuth(tokenOptions);


builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

builder.Services.UseCustomValidationResponse();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomException();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
