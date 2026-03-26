using AuthApi.Domain.Aggregates.Usuario.Dtos;

namespace AuthApi.Domain.Aggregates.Usuario;

public interface IUsuarioDao 
{
    public Task<UsuarioDto?> UsuarioDtoById(Guid id);

    public Task<Usuario?> UsuarioById(Guid id);

    public Task<UsuarioDto?> UsuarioDtoByEmail(string email);

    public Task<Usuario?> UsuarioByEmail(string email);

    public Task<IEnumerable<UsuarioDto?>> ListarUsuarios();

    public Task Adicionar (Usuario usuario);

    public void Update(Usuario usuario);

    public void Remover(Usuario usuario);

    public Task SaveChangesAsync();
}
