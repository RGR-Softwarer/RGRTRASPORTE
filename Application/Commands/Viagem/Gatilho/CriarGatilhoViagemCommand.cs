using Application.Commands.Base;
using Application.Common;
using Dominio.Enums.Data;

namespace Application.Commands.Viagem.Gatilho;

public class CriarGatilhoViagemCommand : BaseCommand<BaseResponse<long>>
{
    public string Descricao { get; set; } = string.Empty;
    public long VeiculoId { get; set; }
    public long MotoristaId { get; set; }
    public long LocalidadeOrigemId { get; set; }
    public long LocalidadeDestinoId { get; set; }
    public TimeSpan HorarioSaida { get; set; }
    public TimeSpan HorarioChegada { get; set; }
    public decimal ValorPassagem { get; set; }
    public int QuantidadeVagas { get; set; }
    public double Distancia { get; set; }
    public string DescricaoViagem { get; set; } = string.Empty;
    public string PolilinhaRota { get; set; } = string.Empty;
    public List<DiaSemanaEnum> DiasSemana { get; set; } = new List<DiaSemanaEnum>();
} 
