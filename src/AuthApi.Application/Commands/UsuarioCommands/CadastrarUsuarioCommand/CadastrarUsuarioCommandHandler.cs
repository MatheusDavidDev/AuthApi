using AuthApi.Core.Utils;
using AuthApi.Domain.Aggregates.Usuario;
using AuthApi.Infra;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;

public class CadastrarUsuarioCommandHandler : IRequestHandler<CadastrarUsuarioCommand, Guid>
{
    private readonly AuthApiDbContext _context;
    private readonly IUsuarioDao _usuarioDao;

    public CadastrarUsuarioCommandHandler(AuthApiDbContext context, IUsuarioDao usuarioDao)
    {
        _context = context;
        _usuarioDao = usuarioDao;
    }

    public async Task<Guid> Handle(CadastrarUsuarioCommand command,CancellationToken cancellationToken)
    {
        var existente = await _usuarioDao.UsuarioByEmail(command.Email);
        if (existente != null)
            throw new Exception("já existe um usuário com esse email");

        var usuario = new Usuario(
            command.Nome,
            command.Email,
            Senha.GerarHash(command.Senha),
            command.Tipo
        );

        _context.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        return usuario.Id;
    }
}

