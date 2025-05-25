using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemPassageiroRepository : GenericRepository<ViagemPassageiro>, IViagemPassageiroRepository
    {
        public ViagemPassageiroRepository(TransportadorContext context) : base(context) { }
    }
}