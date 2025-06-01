using Application.Commands.Veiculo.ModeloVeicular;
using FluentValidation;

namespace Application.Validators.Veiculo.ModeloVeicular;

public class CriarModeloVeicularCommandValidator : AbstractValidator<CriarModeloVeicularCommand>
{
    public CriarModeloVeicularCommandValidator()
    {
        
        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição deve ter no máximo 500 caracteres");

        RuleFor(x => x.QuantidadeAssento)
            .GreaterThan(0)
            .WithMessage("A quantidade de passageiros deve ser maior que zero");
    }
} 