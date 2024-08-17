using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Veiculo
{
    public class ModeloVeicular : BaseEntity
    {
        public  bool Situacao { get; }
        public  bool DescricaoModelo { get; }
        public  TipoModeloVeiculoEnum Tipo { get; }
        public  int QuantidadeAssento { get; }
        public  int QuantidadeEixo { get; }
        public  int CapacidadeMaxima { get; }
        public  int PassageirosEmPe { get; }
        public  bool PossuiBanheiro { get; }
        public  bool PossuiClimatizador { get; }
        public  ICollection<Veiculo> Veiculos { get; }
        public string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }
    }
}
