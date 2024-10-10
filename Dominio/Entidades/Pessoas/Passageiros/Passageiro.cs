using Dominio.Entidades.Localidades;
using Dominio.Enums.Pessoas;

namespace Dominio.Entidades.Pessoas.Passageiros
{
    public class Passageiro : BaseEntity
    {
        public string Nome { get; private set; }
        public bool Situacao { get; private set; }
        public string CPF { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public SexoEnum Sexo { get; private set; }
        public Localidade Localidade { get; private set; }
        public long LocalidadeId { get; private set; }
        public Localidade LocalidadeEmbarque { get; private set; }
        public long LocalidadeEmbarqueId { get; private set; }
        public Localidade LocalidadeDesembarque { get; private set; }
        public long LocalidadeDesembarqueId { get; private set; }
        public string Observacao { get; private set; }
    }
}
