using AuthApi.Domain.Aggregates.Usuario.Dtos;

namespace AuthApi.Domain.Aggregates.Usuario;

public interface IUsuarioDao 
{
    public Task<UsuarioDto?> UsuarioById(Guid id);

    public Task<UsuarioDto?> UsuarioByEmail(string email);

    public Task<IEnumerable<UsuarioDto?>> ListarUsuarios();
}
