using MediatR;
using Dominio.Dtos.Viagens;

namespace Application.Commands.Viagem
{
    public class ObterTodasViagensQuery : IRequest<IEnumerable<ViagemDto>>
    {
    }
} 