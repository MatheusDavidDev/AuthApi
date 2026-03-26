using AuthApi.Application.Responses;
using AuthApi.Core.Commands;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.LoginCommand;

public class LoginCommand : ICommand<LoginResponse>, IRequest<LoginResponse>
{
    public LoginCommand(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    public string Email { get; set; }
    public string Senha { get; set; }
}
