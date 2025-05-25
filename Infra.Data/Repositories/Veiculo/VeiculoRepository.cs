using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data.Veiculo;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories
{
    public class VeiculoRepository : GenericRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(TransportadorContext context) : base(context) { }

    }
}
