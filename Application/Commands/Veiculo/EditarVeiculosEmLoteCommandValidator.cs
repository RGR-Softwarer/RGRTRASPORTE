using FluentValidation;

namespace Application.Commands.Veiculo;

public class EditarVeiculosEmLoteCommandValidator : AbstractValidator<EditarVeiculosEmLoteCommand>
{
    public EditarVeiculosEmLoteCommandValidator()
    {
        RuleFor(x => x.Veiculos)
            .NotEmpty()
            .WithMessage("A lista de veículos é obrigatória");

        RuleForEach(x => x.Veiculos).SetValidator(new VeiculoLoteEdicaoDtoValidator());
    }
}

public class VeiculoLoteEdicaoDtoValidator : AbstractValidator<VeiculoLoteEdicaoDto>
{
    public VeiculoLoteEdicaoDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do veículo é obrigatório")
            .GreaterThan(0)
            .WithMessage("O ID do veículo deve ser maior que zero");

        Include(new VeiculoLoteDtoValidator());
    }
} 
