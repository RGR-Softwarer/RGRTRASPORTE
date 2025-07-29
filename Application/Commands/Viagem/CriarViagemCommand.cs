using Application.Commands.Base;
using Application.Common;

namespace Application.Commands.Viagem;

public class CriarViagemCommand : BaseCommand<BaseResponse<long>>
{
    public DateTime DataViagem { get; private set; }
    public TimeSpan HorarioSaida { get; private set; }
    public TimeSpan HorarioChegada { get; private set; }
    public long VeiculoId { get; private set; }
    public long MotoristaId { get; private set; }
    public long LocalidadeOrigemId { get; private set; }
    public long LocalidadeDestinoId { get; private set; }
    public int QuantidadeVagas { get; private set; }
    public decimal Distancia { get; private set; }
    public string DescricaoViagem { get; private set; }
    public string PolilinhaRota { get; private set; }
    public bool Ativo { get; private set; }
    public long? GatilhoViagemId { get; private set; }

    public CriarViagemCommand(
        DateTime dataViagem,
        TimeSpan horarioSaida,
        TimeSpan horarioChegada,
        long veiculoId,
        long motoristaId,
        long localidadeOrigemId,
        long localidadeDestinoId,
        int quantidadeVagas,
        decimal distancia,
        string descricaoViagem,
        string polilinhaRota,
        bool ativo,
        long? gatilhoViagemId)
    {
        DataViagem = dataViagem;
        HorarioSaida = horarioSaida;
        HorarioChegada = horarioChegada;
        VeiculoId = veiculoId;
        MotoristaId = motoristaId;
        LocalidadeOrigemId = localidadeOrigemId;
        LocalidadeDestinoId = localidadeDestinoId;
        QuantidadeVagas = quantidadeVagas;
        Distancia = distancia;
        DescricaoViagem = descricaoViagem;
        PolilinhaRota = polilinhaRota;
        Ativo = ativo;
        GatilhoViagemId = gatilhoViagemId;
    }
} 