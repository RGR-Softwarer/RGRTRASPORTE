using Dominio.Dtos.Auditoria;
using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Enums;
using Dominio.Enums.Auditoria;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Data.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Atributos Privados

        private readonly RGRContext _context;

        #endregion

        public GenericRepository(RGRContext context)
        {
            _context = context;
        }

        #region Métodos Públicos

        public async Task<List<T>> ObterTodosAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> ObterTodosAsync(int inicioRegistros, int maximoRegistros, string propriedadeOrdenar, bool decrescente = false)
        {
            var query = _context.Set<T>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(propriedadeOrdenar))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, propriedadeOrdenar);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

                query = decrescente ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
            }

            return await query.Skip(inicioRegistros).Take(maximoRegistros).ToListAsync();
        }

        public async Task<T> ObterPorIdAsync(long id, bool auditado = false)
        {
            var registro = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (auditado)
                registro?.Initialize();

            return registro;
        }

        public async Task AdicionarAsync(T entidade, AuditadoDto auditado = null)
        {
            await AddAsync(entidade);

            if (auditado != null)
                AuditarAsync(auditado, entidade, null, AcaoBancoDadosEnum.Insert);
        }

        public async Task AdicionarEmLoteAsync(List<T> listaEntidades)
        {
            await AddManyAsync(listaEntidades);
        }

        public async Task AtualizarAsync(T entidade, AuditadoDto auditado = null)
        {
            Update(entidade);

            if (auditado != null)
                AuditarAsync(auditado, entidade, entidade.GetChanges(), AcaoBancoDadosEnum.Update);
        }

        public async Task AtualizarEmLoteAsync(List<T> listaEntidades)
        {
            UpdateMany(listaEntidades);
        }

        public async Task RemoverAsync(T entidade)
        {
            Remove(entidade);
        }

        public async Task RemoverEmLoteAsync(List<T> listaEntidades)
        {
            RemoveLote(listaEntidades);
        }

        public IQueryable<T> Query() => _context.Set<T>().AsQueryable();

        public async Task<int> ContarTodosAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        #endregion Métodos Públicos

        #region Métodos Privados

        private async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        private async Task AddManyAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        private void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void UpdateMany(List<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
        }

        private void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        private void RemoveLote(List<T> listaEntidades)
        {
            _context.Set<T>().RemoveRange(listaEntidades);
        }

        #endregion Métodos Privados     

        #region Métodos Privados - Auditoria

        private void AuditarAsync(AuditadoDto auditado, T entidade, List<Dominio.Entidades.Auditoria.HistoricoPropriedade> alteracoes, AcaoBancoDadosEnum acao, string descricaoAcao = "")
        {
            var historico = new HistoricoObjeto
            {
                CodigoObjeto = entidade.Id,
                Data = DateTime.UtcNow,
                Objeto = entidade.GetType().Name,
                Acao = acao,
                DescricaoObjeto = entidade.DescricaoAuditoria ?? string.Empty,
                DescricaoAcao = !string.IsNullOrWhiteSpace(descricaoAcao) ? descricaoAcao : acao.ObterDescricao(),
                IP = auditado.IP,
                TipoAuditado = auditado.TipoAuditado,
                OrigemAuditado = auditado.OrigemAuditado
            };

            if (alteracoes?.Count > 0)
                historico.HistoricoPropriedade = alteracoes;

            _context.PendingEntities.Add((entidade, historico));
        }

        #endregion
    }
}