using Application.Commands.Viagem;
using Application.Validators.Base;
using FluentValidation;

namespace Application.Validators.Viagem;

public class CriarViagemCommandValidator : BaseValidator<CriarViagemCommand>
{
    public CriarViagemCommandValidator()
    {
        ValidarDataObrigatoria(RuleFor(x => x.DataViagem), "data da viagem");
        ValidarDataFutura(RuleFor(x => x.DataViagem), "data da viagem");

        RuleFor(x => x.HorarioSaida)
            .Must(h => h != TimeSpan.Zero)
            .WithMessage("O horário de saída é obrigatório");

        RuleFor(x => x.HorarioChegada)
            .Must(h => h != TimeSpan.Zero)
            .WithMessage("O horário de chegada é obrigatório")
            .GreaterThan(x => x.HorarioSaida)
            .WithMessage("O horário de chegada deve ser maior que o horário de saída");

        ValidarIdObrigatorio(RuleFor(x => x.VeiculoId), "veículo");
        ValidarIdObrigatorio(RuleFor(x => x.MotoristaId), "motorista");
        ValidarIdObrigatorio(RuleFor(x => x.LocalidadeOrigemId), "localidade de origem");
        
        ValidarIdObrigatorio(RuleFor(x => x.LocalidadeDestinoId), "localidade de destino");
        
        RuleFor(x => x.LocalidadeDestinoId)
            .NotEqual(x => x.LocalidadeOrigemId)
            .WithMessage("A localidade de destino deve ser diferente da localidade de origem");

        ValidarQuantidadePositiva(RuleFor(x => x.QuantidadeVagas), "quantidade de vagas");
        ValidarValorPositivo(RuleFor(x => x.Distancia), "distância");

        ValidarTextoObrigatorio(RuleFor(x => x.DescricaoViagem), "descrição da viagem", 500);
        ValidarTextoObrigatorio(RuleFor(x => x.PolilinhaRota), "polilinha da rota", 10000);
    }
} 