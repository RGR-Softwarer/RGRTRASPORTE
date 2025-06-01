using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class EditarViagemCommand : BaseCommand<BaseResponse<bool>>
{
    public long Id { get; private set; }
    public DateTime DataViagem { get; private set; }
    public TimeSpan HorarioSaida { get; private set; }
    public TimeSpan HorarioChegada { get; private set; }
    public long VeiculoId { get; private set; }
    public long LocalidadeOrigemId { get; private set; }
    public long LocalidadeDestinoId { get; private set; }
    public decimal ValorPassagem { get; private set; }
    public int QuantidadeVagas { get; private set; }
    public bool Ativo { get; private set; }

    public EditarViagemCommand(
        long id,
        DateTime dataViagem,
        TimeSpan horarioSaida,
        TimeSpan horarioChegada,
        long veiculoId,
        long localidadeOrigemId,
        long localidadeDestinoId,
        decimal valorPassagem,
        int quantidadeVagas,
        bool ativo,
        string usuarioId,
        string usuarioCriacao) : base(usuarioId, usuarioCriacao)
    {
        Id = id;
        DataViagem = dataViagem;
        HorarioSaida = horarioSaida;
        HorarioChegada = horarioChegada;
        VeiculoId = veiculoId;
        LocalidadeOrigemId = localidadeOrigemId;
        LocalidadeDestinoId = localidadeDestinoId;
        ValorPassagem = valorPassagem;
        QuantidadeVagas = quantidadeVagas;
        Ativo = ativo;
    }
} 