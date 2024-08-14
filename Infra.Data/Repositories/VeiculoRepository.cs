using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories
{
    public class VeiculoRepository : GenericRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(RGRContext context) : base(context) { }

    }
}
