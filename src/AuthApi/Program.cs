using AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.LoginCommand;
using AuthApi.Application.Interfaces;
using AuthApi.Core.Behaviors;
using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Infra;
using AuthApi.Infra.Daos;
using AuthApi.Infra.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar o DbContext com a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthApiDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioDao, UsuarioDao>();

// Configurar o MediatR para registrar os handlers e comportamentos
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CadastrarUsuarioCommandHandler).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(EditarTipoUsuarioCommandHandler).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ExcluirUsuarioCommandHandler).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CadastrarUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(EditarTipoUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ExcluirUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

// Configurar versionamento de API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Formato: v1, v1.0
    options.SubstituteApiVersionInUrl = true; 
});

// Configurar autenticação JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<ITokenService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var key = config["Jwt:Key"];

    return new TokenService(key);
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

app.UseAuthentication();

app.MapControllers();

app.Run();



