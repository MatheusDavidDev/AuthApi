using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infra;

public class AuthApiDbContext : DbContext
{
    public AuthApiDbContext(DbContextOptions<AuthApiDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthApiDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioMapping).Assembly);
    }
}
