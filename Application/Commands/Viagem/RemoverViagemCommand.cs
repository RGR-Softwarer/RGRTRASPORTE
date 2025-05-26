using MediatR;

namespace Application.Commands.Viagem
{
    public class RemoverViagemCommand : IRequest<bool>
    {
        public long Id { get; set; }

        public RemoverViagemCommand(long id)
        {
            Id = id;
        }
    }
} 