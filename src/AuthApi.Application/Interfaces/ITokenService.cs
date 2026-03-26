using AuthApi.Domain.Aggregates.Usuario;

namespace AuthApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);

        string GenerateRefreshToken();
    }
}
