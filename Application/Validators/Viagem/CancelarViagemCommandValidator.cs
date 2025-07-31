using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class CancelarViagemCommandValidator : AbstractValidator<CancelarViagemCommand>
{
    public CancelarViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");

        RuleFor(x => x.Motivo)
            .NotEmpty()
            .WithMessage("O motivo do cancelamento é obrigatório")
            .MaximumLength(500)
            .WithMessage("O motivo do cancelamento deve ter no máximo 500 caracteres");
    }
} 
