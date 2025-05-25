using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemRepository : GenericRepository<Viagem>, IViagemRepository
    {
        public ViagemRepository(TransportadorContext context) : base(context) { }
    }
}