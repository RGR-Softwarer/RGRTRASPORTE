using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class EditarViagemCommandValidator : AbstractValidator<EditarViagemCommand>
{
    public EditarViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");

        RuleFor(x => x.DataViagem)
            .NotEmpty()
            .WithMessage("A data da viagem é obrigatória")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("A data da viagem deve ser maior ou igual a data atual");

        RuleFor(x => x.HorarioSaida)
            .NotEmpty()
            .WithMessage("O horário de saída é obrigatório");

        RuleFor(x => x.HorarioChegada)
            .NotEmpty()
            .WithMessage("O horário de chegada é obrigatório")
            .GreaterThan(x => x.HorarioSaida)
            .WithMessage("O horário de chegada deve ser maior que o horário de saída");

        RuleFor(x => x.VeiculoId)
            .GreaterThan(0)
            .WithMessage("O veículo é obrigatório");


        RuleFor(x => x.LocalidadeOrigemId)
            .GreaterThan(0)
            .WithMessage("A localidade de origem é obrigatória");

        RuleFor(x => x.LocalidadeDestinoId)
            .GreaterThan(0)
            .WithMessage("A localidade de destino é obrigatória")
            .NotEqual(x => x.LocalidadeOrigemId)
            .WithMessage("A localidade de destino deve ser diferente da localidade de origem");

        // REMOVIDO: ValorPassagem não existe mais na entidade Viagem

        RuleFor(x => x.QuantidadeVagas)
            .GreaterThan(0)
            .WithMessage("A quantidade de vagas deve ser maior que zero");
    }
} 