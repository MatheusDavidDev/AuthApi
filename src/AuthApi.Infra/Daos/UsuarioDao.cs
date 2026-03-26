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

    public async Task<UsuarioDto?> UsuarioById(Guid id)
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

    public async Task<UsuarioDto?> UsuarioByEmail(string email)
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
}
