using Application.Commands.Veiculo.ModeloVeicular;
using FluentValidation;

namespace Application.Validators.Veiculo.ModeloVeicular;

public class RemoverModeloVeicularCommandValidator : AbstractValidator<RemoverModeloVeicularCommand>
{
    public RemoverModeloVeicularCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do modelo veicular é obrigatório");
    }
} 