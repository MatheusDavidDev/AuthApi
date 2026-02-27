using AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;
using AuthApi.Core.Behaviors;
using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Infra;
using AuthApi.Infra.Daos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CadastrarUsuarioCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CadastrarUsuarioCommandValidator).Assembly);



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

app.Run();
