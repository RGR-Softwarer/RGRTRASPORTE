using MediatR;
using Dominio.Dtos.Viagens;

namespace Application.Commands.Viagem
{
    public class ObterViagemPorIdQuery : IRequest<ViagemDto>
    {
        public long Id { get; set; }

        public ObterViagemPorIdQuery(long id)
        {
            Id = id;
        }
    }
} 