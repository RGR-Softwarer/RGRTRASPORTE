using Dominio.ObjetosDeValor.Logistica;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPosicao : BaseEntity
    {
        protected ViagemPosicao() { } // Construtor protegido para EF Core

        public ViagemPosicao(
            long viagemId,
            DateTime dataHora,
            string latitude,
            string longitude)
        {
            ViagemId = viagemId;
            DataHora = dataHora;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void Atualizar(
            long viagemId,
            DateTime dataHora,
            string latitude,
            string longitude)
        {
            ViagemId = viagemId;
            DataHora = dataHora;
            Latitude = latitude;
            Longitude = longitude;
        }

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
