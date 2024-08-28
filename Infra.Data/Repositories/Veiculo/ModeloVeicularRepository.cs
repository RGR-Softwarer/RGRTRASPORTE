using Dominio.Entidades.Veiculo;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories
{
    public class ModeloVeicularRepository : GenericRepository<ModeloVeicular>, IModeloVeicularRepository
    {
        public ModeloVeicularRepository(RGRContext context) : base(context) { }

    }
}
