using Dominio.Enums.Veiculo;

namespace Dominio.Entidades.Veiculos
{
    public class ModeloVeicular : BaseEntity
    {
        protected ModeloVeicular() { } // Construtor protegido para EF Core

        public ModeloVeicular(
            string descricaoModelo,
            TipoModeloVeiculoEnum tipo,
            int quantidadeAssento,
            int quantidadeEixo,
            int capacidadeMaxima,
            int passageirosEmPe,
            bool possuiBanheiro,
            bool possuiClimatizador,
            bool situacao)
        {
            Descricao = descricaoModelo;
            Tipo = tipo;
            QuantidadeAssento = quantidadeAssento;
            QuantidadeEixo = quantidadeEixo;
            CapacidadeMaxima = capacidadeMaxima;
            PassageirosEmPe = passageirosEmPe;
            PossuiBanheiro = possuiBanheiro;
            PossuiClimatizador = possuiClimatizador;
            Situacao = situacao;
            Veiculos = new List<Veiculo>();
        }

        public bool Situacao { get; private set; }
        public string Descricao { get; private set; }
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

        public void Atualizar(string nome, int quantidadePassageiros, bool ativo)
        {
            Descricao = nome;
            Situacao = ativo;
            QuantidadeAssento = quantidadePassageiros;

        }
    }
}
