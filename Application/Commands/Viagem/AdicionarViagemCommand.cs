using MediatR;
using Dominio.Dtos.Viagens;

namespace Application.Commands.Viagem
{
    public class AdicionarViagemCommand : IRequest<bool>
    {
        public ViagemDto ViagemDto { get; set; }
    }
}
