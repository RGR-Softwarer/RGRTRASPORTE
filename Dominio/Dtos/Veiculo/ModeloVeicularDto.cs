using Dominio.Enums.Veiculo;

namespace Dominio.Dtos.Veiculo
{
    public class ModeloVeicularDto
    {
        public long Id { get; set; }
        public bool Situacao { get; init; }
        public bool DescricaoModelo { get; init; }
        public TipoModeloVeiculoEnum Tipo { get; init; }
        public int QuantidadeAssento { get; init; }
        public int QuantidadeEixo { get; init; }
        public int CapacidadeMaxima { get; init; }
        public int PassageirosEmPe { get; init; }
        public bool PossuiBanheiro { get; init; }
        public bool PossuiClimatizador { get; init; }
    }
}