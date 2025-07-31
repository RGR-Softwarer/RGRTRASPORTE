using Dominio.Entidades.Veiculos;
using FluentValidation;

namespace Dominio.Validators
{
    public class VeiculoValidator : AbstractValidator<Veiculo>
    {
        public VeiculoValidator()
        {
            RuleFor(v => v.Placa)
                .NotNull().WithMessage("A placa é obrigatória.");
           
            RuleFor(v => v.Modelo)
                .NotEmpty().WithMessage("O modelo é obrigatório.");

            RuleFor(v => v.Marca)
                .NotEmpty().WithMessage("A marca é obrigatória.");

            RuleFor(v => v.AnoFabricacao)
                .InclusiveBetween(1900, System.DateTime.Now.Year).WithMessage("Ano inválido.");

            RuleFor(v => v.Cor)
                .NotEmpty().WithMessage("A cor é obrigatória.");

            RuleFor(v => v.Renavam)
                .Length(11).WithMessage("O Renavam deve ter 11 caracteres.");

            RuleFor(v => v.TipoCombustivel)
                .IsInEnum().WithMessage("Tipo de combustível inválido.");
            
            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Status do veículo inválido.");

            RuleFor(v => v.Observacao)
                .MaximumLength(500).WithMessage("A observação não pode ter mais que 500 caracteres.");
        }
    }
}
