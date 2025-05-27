using MediatR;

namespace Application.Commands.Viagem
{
    public class AdicionarViagemCommand : IRequest<bool>
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public DateTime DataPartida { get; set; }
        public DateTime DataChegada { get; set; }
        public int PassageiroId { get; set; }
        public int VeiculoId { get; set; }
        public decimal ValorViagem { get; set; }
    }
}
