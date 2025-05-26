using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Veiculos
{
    public class ModeloVeicular : BaseEntity
    {
        public bool Situacao { get; private set; }
        public string DescricaoModelo { get; private set; }
        public TipoModeloVeiculoEnum Tipo { get; private set; }
        public int QuantidadeAssento { get; private set; }
        public int QuantidadeEixo { get; private set; }
        public int CapacidadeMaxima { get; private set; }
        public int PassageirosEmPe { get; private set; }
        public bool PossuiBanheiro { get; private set; }
        public bool PossuiClimatizador { get; private set; }
        public ICollection<Veiculo> Veiculos { get; private set; }
        public string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }
    }
}
