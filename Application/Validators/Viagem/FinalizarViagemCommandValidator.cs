using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class FinalizarViagemCommandValidator : AbstractValidator<FinalizarViagemCommand>
{
    public FinalizarViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");
    }
}
