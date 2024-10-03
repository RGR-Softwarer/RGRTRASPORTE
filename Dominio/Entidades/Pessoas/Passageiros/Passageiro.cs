using Dominio.Entidades.Localidades;
using Dominio.Enums.Pessoas;

namespace Dominio.Entidades.Pessoas.Passageiros
{
    public class Passageiro : BaseEntity
    {
        public string Nome { get; }
        public bool Situacao { get; }
        public string CPF { get; }
        public string Telefone { get; }
        public string Email { get; }
        public SexoEnum Sexo { get; }
        public Localidade Localidade { get; }
        public long LocalidadeId { get; }
        public Localidade LocalidadeEmbarque { get; }
        public long LocalidadeEmbarqueId { get; }
        public Localidade LocalidadeDesembarque { get; }
        public long LocalidadeDesembarqueId { get; }
        public string Observacao { get; }
    }
}
