using AuthApi.Application.Responses;
using AuthApi.Core.Commands;
using MediatR;

namespace AuthApi.Application.Commands.UsuarioCommands.RefreshTokenCommand;

public class RefreshTokenCommand : ICommand<LoginResponse>, IRequest<LoginResponse>
{
    public RefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; set; }
}
