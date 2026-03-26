using FluentValidation;

namespace AuthApi.Application.Commands.UsuarioCommands.LoginCommand;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O Email do usuário é obrigatório.")
            .EmailAddress();

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha do usuário é obrigatória.");
    }
}
