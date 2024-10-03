using Dominio.ObjetosDeValor.Logistica;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPosicao : BaseEntity
    {
        public virtual Viagem Viagem { get; }
        public long ViagemId { get; }
        public DateTime DataHora { get; }
        public string Latitude { get; }
        public string Longitude { get; }

        public virtual WayPoint ObterWayPoint()
        {
            return new WayPoint { Latitude = Latitude, Longitude = Longitude };
        }
    }
}
