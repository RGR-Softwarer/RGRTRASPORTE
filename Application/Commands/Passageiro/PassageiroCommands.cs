using Dominio.Dtos.Pessoas.Passageiros;
using MediatR;

namespace Application.Commands.Passageiro
{
    public class ObterTodosPassageirosQuery : IRequest<IEnumerable<PassageiroDto>> { }

    public class ObterPassageiroPorIdQuery : IRequest<PassageiroDto>
    {
        public long Id { get; set; }
        public ObterPassageiroPorIdQuery(long id) => Id = id;
    }

    public class AdicionarPassageiroCommand : IRequest<bool>
    {
        public PassageiroDto PassageiroDto { get; set; }
    }

    public class EditarPassageiroCommand : IRequest<bool>
    {
        public PassageiroDto PassageiroDto { get; set; }
    }

    public class RemoverPassageiroCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public RemoverPassageiroCommand(long id) => Id = id;
    }
} 