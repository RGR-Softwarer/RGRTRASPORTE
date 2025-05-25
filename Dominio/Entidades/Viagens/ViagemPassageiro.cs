using Dominio.Entidades.Pessoas.Passageiros;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPassageiro : BaseEntity
    {
        public virtual Viagem Viagem { get; private set; }
        public long ViagemId { get; private set; }
        //public virtual Passageiro Passageiro { get; private set; }
        public long PassageiroId { get; private set; }
    }
}
