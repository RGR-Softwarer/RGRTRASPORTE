using Application.Commands.Viagem.ViagemPosicao;
using FluentValidation;

namespace Application.Validators.Viagem.ViagemPosicao;

public class RemoverViagemPosicaoCommandValidator : AbstractValidator<RemoverViagemPosicaoCommand>
{
    public RemoverViagemPosicaoCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da posição é obrigatório");
    }
} 