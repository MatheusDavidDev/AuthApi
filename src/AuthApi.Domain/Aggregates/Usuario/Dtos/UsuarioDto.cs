namespace AuthApi.Domain.Aggregates.Usuario.Dtos;

public class UsuarioDto
{
    public Guid IdUsuario { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; }
}
