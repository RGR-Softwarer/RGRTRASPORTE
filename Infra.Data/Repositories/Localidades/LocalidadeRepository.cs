using Dominio.Entidades.Localidades;
using Dominio.Interfaces.Infra.Data.Localidades;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Localidades
{
    public class LocalidadeRepository : GenericRepository<Localidade>, ILocalidadeRepository
    {
        public LocalidadeRepository(RGRContext context) : base(context) { }
    }
}