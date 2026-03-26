using FluentValidation;

namespace AuthApi.Application.Commands.UsuarioCommands.CadastrarUsuarioCommand;

public class CadastrarUsuarioCommandValidator : AbstractValidator<CadastrarUsuarioCommand>
{
    public CadastrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O Nome do usuário é obrigatório.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O Email do usuário é obrigatório.")
            .EmailAddress();

        RuleFor(x => x.Senha)
            .MinimumLength(6).WithMessage("a senha do usuário precisa ter 6 caracteres.");
    }
}
