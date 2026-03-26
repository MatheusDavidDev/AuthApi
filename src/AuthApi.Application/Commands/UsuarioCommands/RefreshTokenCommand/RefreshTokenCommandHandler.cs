using AuthApi.Application.Interfaces;
using AuthApi.Application.Responses;
using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.RefreshTokenCommand;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IUsuarioDao _usuarioDao;

    public RefreshTokenCommandHandler(ITokenService tokenService, IUsuarioDao usuarioDao)
    {
        _tokenService = tokenService;
        _usuarioDao = usuarioDao;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioDao.UsuarioByRefreshToken(request.RefreshToken);

        if (usuario == null)
            throw new Exception("Refresh token inválido");

        if (usuario.RefreshTokenExpiryTime < DateTime.UtcNow)
            throw new Exception("Refresh token expirado");

        var newToken = _tokenService.GenerateToken(usuario);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        usuario.RefreshToken = newRefreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        _usuarioDao.Update(usuario);
        await _usuarioDao.SaveChangesAsync();

        return new LoginResponse(newToken, newRefreshToken);

    }

}
