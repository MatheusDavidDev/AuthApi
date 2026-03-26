using AuthApi.Core.Commands;
using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;

public class EditarTipoUsuarioCommand : ICommand, IRequest
{
    public EditarTipoUsuarioCommand(Guid idUsuario, TipoUsuario tipo)
    {
        IdUsuario = idUsuario;
        Tipo = tipo;
    }

    public Guid IdUsuario { get; set; }
    public TipoUsuario Tipo { get; set; }
}
