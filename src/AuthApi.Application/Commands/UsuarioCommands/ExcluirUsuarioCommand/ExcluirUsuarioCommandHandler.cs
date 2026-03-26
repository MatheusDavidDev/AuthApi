using AuthApi.Infra;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;

public class ExcluirUsuarioCommandHandler : IRequestHandler<ExcluirUsuarioCommand>
{
    private readonly AuthApiDbContext _context;

    public Task Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == request.IdUsuario);

        if(usuario == null)
            throw new Exception("Usuário não encontrado.");

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}
