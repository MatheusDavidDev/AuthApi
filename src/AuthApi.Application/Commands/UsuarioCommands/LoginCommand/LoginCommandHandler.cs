using AuthApi.Application.Interfaces;
using AuthApi.Application.Responses;
using AuthApi.Core.Utils;
using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUsuarioDao _usuarioDao;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUsuarioDao usuarioDao, ITokenService tokenService)
    {
        _usuarioDao = usuarioDao;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioDao.UsuarioByEmail(request.Email);

        if (usuario == null)
            throw new Exception("Usuário não encontrado");

        var senhaValida = Senha.VerificarHash(request.Senha, usuario.Senha);

        if (!senhaValida)
            throw new Exception("Senha inválida");

        var token = _tokenService.GenerateToken(usuario);
        var refreshToken = _tokenService.GenerateRefreshToken();

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        _usuarioDao.Update(usuario);
        await _usuarioDao.SaveChangesAsync();

        return new LoginResponse(token, refreshToken);

    }
}
