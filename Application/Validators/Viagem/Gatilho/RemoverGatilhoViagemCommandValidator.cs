using Application.Commands.Viagem.Gatilho;
using FluentValidation;

namespace Application.Validators.Viagem.Gatilho;

public class RemoverGatilhoViagemCommandValidator : AbstractValidator<RemoverGatilhoViagemCommand>
{
    public RemoverGatilhoViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do gatilho é obrigatório");
    }
} 