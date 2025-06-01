using Application.Commands.Veiculo.ModeloVeicular;
using FluentValidation;

namespace Application.Validators.Veiculo.ModeloVeicular;

public class EditarModeloVeicularCommandValidator : AbstractValidator<EditarModeloVeicularCommand>
{
    public EditarModeloVeicularCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do modelo veicular é obrigatório");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória")
            .MaximumLength(100)
            .WithMessage("A descrição deve ter no máximo 100 caracteres");

        RuleFor(x => x.QuantidadePassageiros)
            .GreaterThan(0)
            .WithMessage("A quantidade de passageiros deve ser maior que zero");
    }
} 