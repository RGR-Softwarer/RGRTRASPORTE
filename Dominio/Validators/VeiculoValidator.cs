using Dominio.Entidades;
using FluentValidation;

namespace Dominio.Validators
{
    public class VeiculoValidator : AbstractValidator<Veiculo>
    {
        public VeiculoValidator()
        {
            RuleFor(v => v.Placa)
                .NotEmpty().WithMessage("A placa é obrigatória.")
                .Length(7).WithMessage("A placa deve ter 7 caracteres.")
                .Matches("^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$").WithMessage("Formato de placa inválido. Formatos aceitos: antigo (AAA1234) ou Mercosul (AAA1A23).");

            RuleFor(v => v.Modelo)
                .NotEmpty().WithMessage("O modelo é obrigatório.");

            RuleFor(v => v.Marca)
                .NotEmpty().WithMessage("A marca é obrigatória.");

            RuleFor(v => v.Ano)
                .InclusiveBetween(1900, System.DateTime.Now.Year).WithMessage("Ano inválido.");

            RuleFor(v => v.Cor)
                .NotEmpty().WithMessage("A cor é obrigatória.");

            RuleFor(v => v.Renavam)
                .Length(11).WithMessage("O Renavam deve ter 11 caracteres.");

            RuleFor(v => v.TipoCombustivel)
                .IsInEnum().WithMessage("Tipo de combustível inválido.");

            RuleFor(v => v.TipoVeiculo)
                .IsInEnum().WithMessage("Tipo de veículo inválido.");

            RuleFor(v => v.Capacidade)
                .NotEmpty().WithMessage("A capacidade é obrigatória.");

            RuleFor(v => v.CategoriaCNH)
                .IsInEnum().WithMessage("Categoria da CNH inválida.");

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Status do veículo inválido.");

            RuleFor(v => v.Observacao)
                .MaximumLength(500).WithMessage("A observação não pode ter mais que 500 caracteres.");
        }
    }
}