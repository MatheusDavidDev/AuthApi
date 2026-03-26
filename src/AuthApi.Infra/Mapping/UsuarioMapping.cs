using AuthApi.Domain.Aggregates.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Infra.Mapping;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200); 

        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(300);


        builder.Property(x => x.Tipo)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(550)
            .IsRequired(false);

        builder.Property(x => x.RefreshTokenExpiryTime)
            .IsRequired(false);
    }
}
