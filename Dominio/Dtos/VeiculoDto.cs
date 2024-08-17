using Dominio.Enums.Veiculo;

namespace Dominio.Dtos
{
    public class VeiculoDto
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }
        public string Renavam { get; set; }
        public TipoCombustivelEnum TipoCombustivel { get; set; }
        public TipoVeiculoEnum TipoVeiculo { get; set; }
        public string Capacidade { get; set; }
        public CategoriaCNHEnum CategoriaCNH { get; set; }
        public StatusVeiculoEnum Status { get; set; }
        public string Observacao { get; set; }
    }
}
