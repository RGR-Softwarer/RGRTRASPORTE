using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens.Gatilho
{
    public class GatilhoViagemRepository : GenericRepository<GatilhoViagem>, IGatilhoViagemRepository
    {
        public GatilhoViagemRepository(RGRContext context) : base(context) { }
    }
}