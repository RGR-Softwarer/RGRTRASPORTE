using Dominio.Entidades.Pessoas.Passageiros;

namespace Dominio.Entidades.Viagens
{
    public class ViagemPassageiro : BaseEntity
    {
        protected ViagemPassageiro() { } // Construtor protegido para EF Core

        public ViagemPassageiro(long viagemId, long passageiroId)
        {
            ViagemId = viagemId;
            PassageiroId = passageiroId;
        }

        public void Atualizar(long viagemId, long passageiroId)
        {
            ViagemId = viagemId;
            PassageiroId = passageiroId;
        }

        public virtual Viagem Viagem { get; private set; }
        public long ViagemId { get; private set; }
        public virtual Passageiro Passageiro { get; private set; }
        public long PassageiroId { get; private set; }
    }
}
