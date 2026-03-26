using AuthApi.Infra;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;

public class EditarTipoUsuarioCommandHandler : IRequestHandler<EditarTipoUsuarioCommand>
{
    private readonly AuthApiDbContext _context;

    public EditarTipoUsuarioCommandHandler(AuthApiDbContext context)
    {
        _context = context;
    }

    public Task Handle(EditarTipoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == request.IdUsuario);
        if (usuario == null)
            throw new Exception("Usuário não encontrado");

        if (usuario.Tipo == request.Tipo)
            throw new Exception("O tipo do usuário já é o mesmo");

        usuario.Tipo = request.Tipo;
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}
