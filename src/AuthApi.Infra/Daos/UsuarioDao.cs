using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Domain.Aggregates.Usuario.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infra.Daos;

public class UsuarioDao : IUsuarioDao
{
    private readonly AuthApiDbContext _authDbContext;

    public UsuarioDao(AuthApiDbContext authApiDbContext)
    {
        _authDbContext = authApiDbContext;
    }

    public async Task<UsuarioDto?> UsuarioDtoById(Guid id)
    {
        return await _authDbContext.Usuarios
                            .Where(u => u.Id == id)
                            .Select(u => new UsuarioDto
                            {
                                IdUsuario = u.Id,
                                Nome = u.Nome,
                                Email = u.Email,
                                Tipo = u.Tipo.ToString()
                            }).FirstOrDefaultAsync();
    }

    public async Task<Usuario?> UsuarioById(Guid id)
    {
        return await _authDbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UsuarioDto?> UsuarioDtoByEmail(string email)
    {
        return await _authDbContext.Usuarios
                            .Where(u => u.Email == email)
                            .Select(u => new UsuarioDto
                            {
                                IdUsuario = u.Id,
                                Nome = u.Nome,
                                Email = u.Email,
                                Tipo = u.Tipo.ToString()
                            }).FirstOrDefaultAsync();
    }

    public async Task<Usuario?> UsuarioByEmail(string email)
    {
        return await _authDbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<UsuarioDto?>> ListarUsuarios()
    {
        return await _authDbContext.Usuarios
             .Select(u => new UsuarioDto
             {
                 IdUsuario = u.Id,
                 Nome = u.Nome,
                 Email = u.Email,
                 Tipo = u.Tipo.ToString()
             }).ToListAsync();
    }

    public async Task Adicionar(Usuario usuario)
    {
        await _authDbContext.Usuarios.AddAsync(usuario);
    }

    public void Update(Usuario usuario)
    {
         _authDbContext.Update(usuario);
    }

    public void Remover(Usuario usuario)
    {
        _authDbContext.Usuarios.Remove(usuario);
    }

    public async Task SaveChangesAsync()
    {
        await _authDbContext.SaveChangesAsync();
    }
}
