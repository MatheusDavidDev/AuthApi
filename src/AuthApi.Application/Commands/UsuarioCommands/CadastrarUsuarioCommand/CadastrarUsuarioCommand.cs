using AuthApi.Core.Commands;
using AuthApi.Domain.Aggregates.Usuario;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;

public class CadastrarUsuarioCommand : ICommand<Guid>, IRequest<Guid>
{
    public CadastrarUsuarioCommand(string nome, string email, string senha, TipoUsuario tipo)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Tipo = tipo;
    }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuario Tipo { get; set; }
}
