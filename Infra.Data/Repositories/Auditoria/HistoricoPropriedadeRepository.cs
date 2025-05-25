using Dominio.Entidades.Auditoria;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Auditoria
{
    public class HistoricoPropriedadeRepository : GenericRepository<HistoricoPropriedade>//, IVeiculoRepository
    {
        public HistoricoPropriedadeRepository(TransportadorContext context) : base(context) { }
    }
}
