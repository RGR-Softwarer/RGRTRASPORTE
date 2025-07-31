using Dominio.Enums.Pessoas;

namespace Dominio.Dtos.Pessoas.Passageiros
{
    public class PassageiroDto
    {
        public long Id { get; set; }
        public string Nome { get; init; }
        public bool Situacao { get; init; }
        public string CPF { get; init; }
        public string Telefone { get; init; }
        public string Email { get; init; }
        public SexoEnum Sexo { get; init; }
        public long LocalidadeId { get; init; }
        public long LocalidadeEmbarqueId { get; init; }
        public long LocalidadeDesembarqueId { get; init; }
        public string Observacao { get; init; }
    }
}
