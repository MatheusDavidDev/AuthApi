using AuthApi.Application.Interfaces;
using AuthApi.Core.Utils;
using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUsuarioDao _usuarioDao;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUsuarioDao usuarioDao, ITokenService tokenService)
    {
        _usuarioDao = usuarioDao;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioDao.UsuarioByEmail(request.Email);

        if (usuario == null)
            throw new Exception("Usuário não encontrado");

        var senhaValida = Senha.VerificarHash(request.Senha, usuario.Senha);

        if (!senhaValida)
            throw new Exception("Senha inválida");

        return _tokenService.GenerateToken(usuario);
    }
}
