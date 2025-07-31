using FluentValidation;

namespace Application.Commands.Veiculo;

public class RemoverVeiculoCommandValidator : AbstractValidator<RemoverVeiculoCommand>
{
    public RemoverVeiculoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do veículo é obrigatório")
            .GreaterThan(0)
            .WithMessage("O ID do veículo deve ser maior que zero");
    }
} 
