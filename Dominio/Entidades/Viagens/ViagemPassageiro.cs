using Dominio.Entidades.Pessoas.Passageiros;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPassageiro : BaseEntity
    {
        public virtual Viagem Viagem { get; }
        public long ViagemId { get; }
        public virtual Passageiro Passageiro { get; }
        public long PassageiroId { get; }
    }
}
