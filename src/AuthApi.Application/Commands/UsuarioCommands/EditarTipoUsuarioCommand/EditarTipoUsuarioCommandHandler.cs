using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;

public class EditarTipoUsuarioCommandHandler : IRequestHandler<EditarTipoUsuarioCommand>
{
    private readonly IUsuarioDao _usuarioDao;

    public EditarTipoUsuarioCommandHandler(IUsuarioDao usuarioDao)
    {
        _usuarioDao = usuarioDao;
    }

    public async Task Handle(EditarTipoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioDao.UsuarioById(request.IdUsuario);
        if (usuario == null)
            throw new Exception("Usuário não encontrado");

        if (usuario.Tipo == request.Tipo)
            throw new Exception("O tipo do usuário já é o mesmo");

        usuario.Tipo = request.Tipo;

        _usuarioDao.Update(usuario);

        await _usuarioDao.SaveChangesAsync();

    }
}
