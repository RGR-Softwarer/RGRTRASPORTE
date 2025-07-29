using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Passageiros
{
    public class PassageiroRepository : GenericRepository<Passageiro>, IPassageiroRepository
    {
        public PassageiroRepository(IUnitOfWorkContext unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Passageiro?> ObterPassageiroCompletoAsync(long id)
        {
            return await Query()
                .Include(p => p.Localidade)
                .Include(p => p.LocalidadeEmbarque)
                .Include(p => p.LocalidadeDesembarque)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}