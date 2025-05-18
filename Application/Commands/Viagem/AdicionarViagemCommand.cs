using MediatR;

namespace Application.Commands.ViagemPassageiro
{
    public class AdicionarViagemCommand : IRequest<Unit>
    {
        public string CodigoViagem { get; set; }
        public DateTime DataViagem { get; set; }
        public long VeiculoId { get; set; }
        public long MotoristaId { get; set; }
        public long OrigemId { get; set; }
        public long DestinoId { get; set; }
        public long GatinhoViagemId { get; set; }
        public DateTime HorarioSaida { get; set; }
        public DateTime HorarioChegada { get; set; }
        public string Situacao { get; set; }
        public string MotivoProblema { get; set; }
        public string DescricaoViagem { get; set; }
        public DateTime DataInicioViagem { get; set; }
        public double LatitudeInicioViagem { get; set; }
        public double LongitudeInicioViagem { get; set; }
        public DateTime DataFimViagem { get; set; }
        public double LatitudeFimViagem { get; set; }
        public double LongitudeFimViagem { get; set; }
        public double Distancia { get; set; }
        public double DistanciaRealizada { get; set; }
        public string PolilinhaRota { get; set; }
        public string PolilinhaRotaRealizada { get; set; }
        public int NumeroPassageiros { get; set; }       
    }
}
