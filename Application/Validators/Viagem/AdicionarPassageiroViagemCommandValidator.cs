using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class AdicionarPassageiroViagemCommandValidator : AbstractValidator<AdicionarPassageiroViagemCommand>
{
    public AdicionarPassageiroViagemCommandValidator()
    {
        RuleFor(x => x.ViagemId)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");

        RuleFor(x => x.PassageiroId)
            .GreaterThan(0)
            .WithMessage("O ID do passageiro é obrigatório");
    }
} 
