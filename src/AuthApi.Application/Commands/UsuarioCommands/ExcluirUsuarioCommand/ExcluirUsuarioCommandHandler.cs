using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;

public class ExcluirUsuarioCommandHandler : IRequestHandler<ExcluirUsuarioCommand>
{
    private readonly IUsuarioDao _usuarioDao;

    public ExcluirUsuarioCommandHandler(IUsuarioDao usuarioDao)
    {
        _usuarioDao = usuarioDao;
    }

    public async Task Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioDao.UsuarioById(request.IdUsuario);

        if(usuario == null)
            throw new Exception("Usuário não encontrado.");

        _usuarioDao.Remover(usuario);

        await _usuarioDao.SaveChangesAsync();

    }
}
