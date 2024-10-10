using Dominio.ObjetosDeValor.Logistica;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPosicao : BaseEntity
    {
        public virtual Viagem Viagem { get; private set; }
        public long ViagemId { get; private set; }
        public DateTime DataHora { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        public virtual WayPoint ObterWayPoint()
        {
            return new WayPoint { Latitude = Latitude, Longitude = Longitude };
        }
    }
}
