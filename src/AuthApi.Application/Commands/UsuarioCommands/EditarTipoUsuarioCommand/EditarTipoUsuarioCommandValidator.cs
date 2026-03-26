using FluentValidation;

namespace AuthApi.Application.Commands.UsuarioCommands.EditarUsuarioCommand;

public class EditarTipoUsuarioCommandValidator : AbstractValidator<EditarTipoUsuarioCommand>
{
    public EditarTipoUsuarioCommandValidator()
    {
        RuleFor(x => x.IdUsuario)
            .NotEmpty().WithMessage("O Id do usuário é obrigatório.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("O Tipo do usuário é obrigatório.");
    }
}
