using Dominio.Entidades.Veiculos;
using FluentValidation;

namespace Dominio.Validators
{
    public class VeiculoValidator : AbstractValidator<Veiculo>
    {
        public VeiculoValidator()
        {
            RuleFor(v => v.Placa)
                .NotNull().WithMessage("A placa � obrigat�ria.");
           
            RuleFor(v => v.Modelo)
                .NotEmpty().WithMessage("O modelo � obrigat�rio.");

            RuleFor(v => v.Marca)
                .NotEmpty().WithMessage("A marca � obrigat�ria.");

            RuleFor(v => v.AnoFabricacao)
                .InclusiveBetween(1900, System.DateTime.Now.Year).WithMessage("Ano inv�lido.");

            RuleFor(v => v.Cor)
                .NotEmpty().WithMessage("A cor � obrigat�ria.");

            RuleFor(v => v.Renavam)
                .Length(11).WithMessage("O Renavam deve ter 11 caracteres.");

            RuleFor(v => v.TipoCombustivel)
                .IsInEnum().WithMessage("Tipo de combust�vel inv�lido.");
            
            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Status do ve�culo inv�lido.");

            RuleFor(v => v.Observacao)
                .MaximumLength(500).WithMessage("A observa��o n�o pode ter mais que 500 caracteres.");
        }
    }
}
