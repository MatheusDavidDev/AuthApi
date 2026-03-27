using AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;
using AuthApi.Application.Commands.UsuarioCommands.LoginCommand;
using AuthApi.Application.Commands.UsuarioCommands.RefreshTokenCommand;
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
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });

    //CONFIG JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Digite: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RefreshTokenCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CadastrarUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(EditarTipoUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ExcluirUsuarioCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(RefreshTokenCommandValidator).Assembly);

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
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("Jwt:Key não configurada");

var key = Encoding.UTF8.GetBytes(jwtKey);

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
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
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

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



