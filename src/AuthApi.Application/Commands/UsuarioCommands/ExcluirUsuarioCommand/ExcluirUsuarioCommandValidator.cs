using FluentValidation;

namespace AuthApi.Application.Commands.UsuarioCommands.ExcluirUsuarioCommand;

public class ExcluirUsuarioCommandValidator : AbstractValidator<ExcluirUsuarioCommand>
{
    public ExcluirUsuarioCommandValidator()
    {
        RuleFor(x => x.IdUsuario)
            .NotEmpty().WithMessage("O Id do usuário é obrigatório.");
    }
}
