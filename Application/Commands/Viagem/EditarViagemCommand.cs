using MediatR;
using Dominio.Dtos.Viagens;

namespace Application.Commands.Viagem
{
    public class EditarViagemCommand : IRequest<bool>
    {
        public ViagemDto ViagemDto { get; set; }

        public EditarViagemCommand(ViagemDto viagemDto)
        {
            ViagemDto = viagemDto;
        }
    }
} 