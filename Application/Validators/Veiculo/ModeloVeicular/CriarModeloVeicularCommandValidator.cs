using Application.Commands.Veiculo.ModeloVeicular;
using FluentValidation;

namespace Application.Validators.Veiculo.ModeloVeicular;

public class CriarModeloVeicularCommandValidator : AbstractValidator<CriarModeloVeicularCommand>
{
    public CriarModeloVeicularCommandValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória")
            .MaximumLength(100)
            .WithMessage("A descrição deve ter no máximo 100 caracteres");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("O tipo do modelo veicular é inválido");

        RuleFor(x => x.QuantidadeAssento)
            .GreaterThan(0)
            .WithMessage("A quantidade de assentos deve ser maior que zero");

        RuleFor(x => x.QuantidadeEixo)
            .GreaterThan(0)
            .WithMessage("A quantidade de eixos deve ser maior que zero");

        RuleFor(x => x.CapacidadeMaxima)
            .GreaterThan(0)
            .WithMessage("A capacidade máxima deve ser maior que zero");

        RuleFor(x => x.PassageirosEmPe)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade de passageiros em pé não pode ser negativa");

        RuleFor(x => x.CapacidadeMaxima)
            .GreaterThanOrEqualTo(x => x.QuantidadeAssento + x.PassageirosEmPe)
            .WithMessage("A capacidade máxima deve ser maior ou igual à soma de assentos e passageiros em pé");

        RuleFor(x => x.PossuiBanheiro)
            .NotNull()
            .WithMessage("A presença de banheiro é obrigatória");

        RuleFor(x => x.PossuiClimatizador)
            .NotNull()
            .WithMessage("A presença de climatizador é obrigatória");
    }
} 