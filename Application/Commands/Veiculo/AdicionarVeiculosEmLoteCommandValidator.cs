using FluentValidation;

namespace Application.Commands.Veiculo;

public class AdicionarVeiculosEmLoteCommandValidator : AbstractValidator<AdicionarVeiculosEmLoteCommand>
{
    public AdicionarVeiculosEmLoteCommandValidator()
    {
        RuleFor(x => x.Veiculos)
            .NotEmpty()
            .WithMessage("A lista de veículos é obrigatória");

        RuleForEach(x => x.Veiculos).SetValidator(new VeiculoLoteDtoValidator());
    }
}

public class VeiculoLoteDtoValidator : AbstractValidator<VeiculoLoteDto>
{
    public VeiculoLoteDtoValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty()
            .WithMessage("A placa é obrigatória")
            .Length(7)
            .WithMessage("A placa deve ter 7 caracteres")
            .Matches(@"^[A-Z]{3}[0-9]{4}$")
            .WithMessage("A placa deve estar no formato AAA9999");

        RuleFor(x => x.Modelo)
            .NotEmpty()
            .WithMessage("O modelo é obrigatório")
            .MaximumLength(100)
            .WithMessage("O modelo deve ter no máximo 100 caracteres");

        RuleFor(x => x.Marca)
            .NotEmpty()
            .WithMessage("A marca é obrigatória")
            .MaximumLength(100)
            .WithMessage("A marca deve ter no máximo 100 caracteres");

        RuleFor(x => x.NumeroChassi)
            .NotEmpty()
            .WithMessage("O chassi é obrigatório")
            .Length(17)
            .WithMessage("O chassi deve ter 17 caracteres")
            .Matches(@"^[A-HJ-NPR-Z0-9]{17}$")
            .WithMessage("O chassi deve conter apenas letras (exceto I, O e Q) e números");

        RuleFor(x => x.AnoModelo)
            .NotEmpty()
            .WithMessage("O ano do modelo é obrigatório")
            .GreaterThan(1900)
            .WithMessage("O ano do modelo deve ser maior que 1900")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("O ano do modelo não pode ser maior que o próximo ano");

        RuleFor(x => x.AnoFabricacao)
            .NotEmpty()
            .WithMessage("O ano de fabricação é obrigatório")
            .GreaterThan(1900)
            .WithMessage("O ano de fabricação deve ser maior que 1900")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("O ano de fabricação não pode ser maior que o ano atual");

        RuleFor(x => x.Cor)
            .NotEmpty()
            .WithMessage("A cor é obrigatória")
            .MaximumLength(50)
            .WithMessage("A cor deve ter no máximo 50 caracteres");

        RuleFor(x => x.Renavam)
            .NotEmpty()
            .WithMessage("O renavam é obrigatório")
            .Length(11)
            .WithMessage("O renavam deve ter 11 caracteres")
            .Matches(@"^[0-9]{11}$")
            .WithMessage("O renavam deve conter apenas números");

        RuleFor(x => x.VencimentoLicenciamento)
            .Must(x => !x.HasValue || x.Value > DateTime.Now)
            .WithMessage("A data de vencimento do licenciamento deve ser maior que a data atual");

        RuleFor(x => x.TipoCombustivel)
            .IsInEnum()
            .WithMessage("O tipo de combustível informado é inválido");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("O status informado é inválido");

        RuleFor(x => x.Observacao)
            .MaximumLength(500)
            .WithMessage("A observação deve ter no máximo 500 caracteres");

        RuleFor(x => x.ModeloVeiculoId)
            .Must(x => !x.HasValue || x.Value > 0)
            .WithMessage("O ID do modelo do veículo deve ser maior que zero quando informado");
    }
} 
