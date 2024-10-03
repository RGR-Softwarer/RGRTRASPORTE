using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens.Gatilho
{
    public class GatinhoViagemRepository : GenericRepository<GatilhoViagem>, IGatilhoViagemRepository
    {
        public GatinhoViagemRepository(RGRContext context) : base(context) { }
    }
}