using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Pessoas;
using Dominio.Interfaces.Infra.Data.Motorista;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Motorista
{
    public class MotoristaRepository : GenericRepository<Dominio.Entidades.Pessoas.Motorista>, IMotoristaRepository
    {
        public MotoristaRepository(CadastroContext context) : base(context) { }

        public async Task<Dominio.Entidades.Pessoas.Motorista> ObterMotoristaPorCpfAsync(string cpf)
        {
            return await Query()
                .FirstOrDefaultAsync(m => m.CPF == cpf);
        }

        public async Task<bool> ExisteMotoristaPorCpfAsync(string cpf, long? idExcluir = null)
        {
            var query = Query().Where(m => m.CPF == cpf);

            if (idExcluir.HasValue)
                query = query.Where(m => m.Id != idExcluir.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteMotoristaPorCnhAsync(string cnh, long? idExcluir = null)
        {
            var query = Query().Where(m => m.CNH == cnh);

            if (idExcluir.HasValue)
                query = query.Where(m => m.Id != idExcluir.Value);

            return await query.AnyAsync();
        }
    }
} 