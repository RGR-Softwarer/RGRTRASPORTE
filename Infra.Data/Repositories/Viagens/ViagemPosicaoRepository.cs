using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemPosicaoRepository : GenericRepository<ViagemPosicao>, IViagemPosicaoRepository
    {
        public ViagemPosicaoRepository(TransportadorContext context) : base(context) { }
    }
}