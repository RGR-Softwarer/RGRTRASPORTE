using Dominio.Dtos.Localidades;
using MediatR;

namespace Application.Commands.Localidade
{
    public class ObterTodasLocalidadesQuery : IRequest<IEnumerable<LocalidadeDto>> { }

    public class ObterLocalidadePorIdQuery : IRequest<LocalidadeDto>
    {
        public long Id { get; set; }
        public ObterLocalidadePorIdQuery(long id) => Id = id;
    }

    public class AdicionarLocalidadeCommand : IRequest<bool>
    {
        public LocalidadeDto LocalidadeDto { get; set; }
    }

    public class EditarLocalidadeCommand : IRequest<bool>
    {
        public LocalidadeDto LocalidadeDto { get; set; }
    }

    public class RemoverLocalidadeCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public RemoverLocalidadeCommand(long id) => Id = id;
    }
} 