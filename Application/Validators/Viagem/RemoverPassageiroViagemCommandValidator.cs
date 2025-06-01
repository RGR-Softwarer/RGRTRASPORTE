using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class RemoverPassageiroViagemCommandValidator : AbstractValidator<RemoverPassageiroViagemCommand>
{
    public RemoverPassageiroViagemCommandValidator()
    {
        RuleFor(x => x.ViagemId)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");

        RuleFor(x => x.PassageiroId)
            .GreaterThan(0)
            .WithMessage("O ID do passageiro é obrigatório");
    }
} 