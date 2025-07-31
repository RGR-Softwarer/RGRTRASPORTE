using Dominio.Events.Base;

namespace Dominio.Events.Viagens
{
    public class PosicaoAdicionadaEvent : DomainEvent
    {
        public long ViagemId { get; private set; }
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public DateTime DataHora { get; private set; }

        public PosicaoAdicionadaEvent(long viagemId, decimal latitude, decimal longitude, DateTime dataHora)
        {
            ViagemId = viagemId;
            Latitude = latitude;
            Longitude = longitude;
            DataHora = dataHora;
        }
    }
} 
