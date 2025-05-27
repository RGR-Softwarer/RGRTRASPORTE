using FluentValidation;
using Application.Commands.Viagem;

namespace Application.Validators
{
    public class AdicionarViagemValidator : AbstractValidator<AdicionarViagemCommand>
    {
        public AdicionarViagemValidator()
        {
            RuleFor(x => x.Origem)
                .NotEmpty()
                .WithMessage("A origem da viagem é obrigatória")
                .MaximumLength(100)
                .WithMessage("A origem não pode ter mais que 100 caracteres");

            RuleFor(x => x.Destino)
                .NotEmpty()
                .WithMessage("O destino da viagem é obrigatório")
                .MaximumLength(100)
                .WithMessage("O destino não pode ter mais que 100 caracteres")
                .Must((command, destino) => destino != command.Origem)
                .WithMessage("O destino não pode ser igual à origem");

            RuleFor(x => x.DataPartida)
                .NotEmpty()
                .WithMessage("A data de partida é obrigatória")
                .Must(data => data > DateTime.Now)
                .WithMessage("A data de partida deve ser maior que a data atual");

            RuleFor(x => x.DataChegada)
                .NotEmpty()
                .WithMessage("A data de chegada é obrigatória")
                .Must((command, dataChegada) => dataChegada > command.DataPartida)
                .WithMessage("A data de chegada deve ser maior que a data de partida");

            RuleFor(x => x.PassageiroId)
                .GreaterThan(0)
                .WithMessage("O passageiro é obrigatório");

            RuleFor(x => x.VeiculoId)
                .GreaterThan(0)
                .WithMessage("O veículo é obrigatório");

            RuleFor(x => x.ValorViagem)
                .GreaterThan(0)
                .WithMessage("O valor da viagem deve ser maior que zero")
                .LessThan(10000)
                .WithMessage("O valor da viagem não pode ser maior que R$ 10.000,00");
        }
    }
} 