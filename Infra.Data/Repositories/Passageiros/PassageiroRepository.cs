using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Passageiros
{
    public class PassageiroRepository : GenericRepository<Passageiro>, IPassageiroRepository
    {
        public PassageiroRepository(RGRContext context) : base(context) { }
    }
}