using AuthApi.Core.Models;

namespace AuthApi.Domain.Aggregates.Usuario;

public class Usuario : Entity
{
    public Usuario(string nome, string email, string senha, TipoUsuario tipo)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Tipo = tipo;
    }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuario Tipo { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}
