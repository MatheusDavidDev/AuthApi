using AuthApi.Core.Commands;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;

public class ExcluirUsuarioCommand : ICommand, IRequest
{
    public ExcluirUsuarioCommand(Guid idUsuario)
    {
        IdUsuario = idUsuario;
    }

    public Guid IdUsuario { get; set; }
}
