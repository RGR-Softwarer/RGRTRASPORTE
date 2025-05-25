using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data.Veiculo;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories
{
    public class ModeloVeicularRepository : GenericRepository<ModeloVeicular>, IModeloVeicularRepository
    {
        public ModeloVeicularRepository(TransportadorContext context) : base(context) { }

    }
}
