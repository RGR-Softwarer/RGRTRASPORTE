using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class CriarViagemCommandValidator : AbstractValidator<CriarViagemCommand>
{
    public CriarViagemCommandValidator()
    {
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

        RuleFor(x => x.MotoristaId)
            .GreaterThan(0)
            .WithMessage("O motorista é obrigatório");

        RuleFor(x => x.LocalidadeOrigemId)
            .GreaterThan(0)
            .WithMessage("A localidade de origem é obrigatória");

        RuleFor(x => x.LocalidadeDestinoId)
            .GreaterThan(0)
            .WithMessage("A localidade de destino é obrigatória")
            .NotEqual(x => x.LocalidadeOrigemId)
            .WithMessage("A localidade de destino deve ser diferente da localidade de origem");

        RuleFor(x => x.ValorPassagem)
            .GreaterThan(0)
            .WithMessage("O valor da passagem deve ser maior que zero");

        RuleFor(x => x.QuantidadeVagas)
            .GreaterThan(0)
            .WithMessage("A quantidade de vagas deve ser maior que zero");

        RuleFor(x => x.Distancia)
            .GreaterThan(0)
            .WithMessage("A distância deve ser maior que zero");

        RuleFor(x => x.DescricaoViagem)
            .NotEmpty()
            .WithMessage("A descrição da viagem é obrigatória")
            .MaximumLength(500)
            .WithMessage("A descrição da viagem deve ter no máximo 500 caracteres");

        RuleFor(x => x.PolilinhaRota)
            .NotEmpty()
            .WithMessage("A rota é obrigatória");
    }
} 