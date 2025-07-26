using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Infra.Data.Context;
using Infra.Data.Data;
using System.Linq.Expressions;
using Dominio.Dtos.Auditoria;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemPassageiroRepository : GenericRepository<ViagemPassageiro>, IViagemPassageiroRepository
    {
        public ViagemPassageiroRepository(TransportadorContext context) : base(context)
        {
        }

        public async Task<ViagemPassageiro> ObterViagemPassageiroCompletoAsync(long id)
        {
            return await Query()
                .Include(vp => vp.Viagem)
                .FirstOrDefaultAsync(vp => vp.Id == id);
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorViagemAsync(long viagemId)
        {
            return await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.ViagemId == viagemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterViagensPorPassageiroAsync(long passageiroId)
        {
            return await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.PassageiroId == passageiroId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.Viagem.DataViagem >= dataInicio && vp.Viagem.DataViagem <= dataFim)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorConfirmacaoAsync(bool confirmado)
        {
            return await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.Confirmado == confirmado)
                .ToListAsync();
        }

        public async Task<int> ObterQuantidadePassageirosPorViagemAsync(long viagemId)
        {
            return await Query()
                .CountAsync(vp => vp.ViagemId == viagemId);
        }

        public async Task<int> ObterQuantidadeViagensPorPassageiroAsync(long passageiroId)
        {
            return await Query()
                .CountAsync(vp => vp.PassageiroId == passageiroId);
        }

        public async Task<bool> ExistePassageiroNaViagemAsync(long viagemId, long passageiroId)
        {
            return await Query()
                .AnyAsync(vp => vp.ViagemId == viagemId && vp.PassageiroId == passageiroId);
        }

        public async Task<bool> ExisteViagemComPassageiroAsync(long passageiroId)
        {
            return await Query()
                .AnyAsync(vp => vp.PassageiroId == passageiroId);
        }

        public IQueryable<ViagemPassageiro> Query()
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<ViagemPassageiro> Items, int Total)> GetPaginatedAsync(int pageNumber, int pageSize, string orderByProperty = "", bool isDescending = false, Expression<Func<ViagemPassageiro, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViagemPassageiro>> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ViagemPassageiro> ObterPorIdAsync(long id, bool auditado = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarEmLoteAsync(List<ViagemPassageiro> listaEntidades, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(ViagemPassageiro entidade)
        {
            throw new NotImplementedException();
        }

        public Task RemoverEmLoteAsync(List<ViagemPassageiro> listaEntidades)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarAsync(ViagemPassageiro entidade, AuditadoDto auditado = null)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarEmLoteAsync(List<ViagemPassageiro> listaEntidades)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarAsync(ViagemPassageiro entidade, AuditadoDto auditado = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}