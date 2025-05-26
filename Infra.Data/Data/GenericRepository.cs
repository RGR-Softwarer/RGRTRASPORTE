using Dominio.Dtos.Auditoria;
using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Enums.Auditoria;
using Dominio.Interfaces.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Data.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Atributos Privados

        private readonly IUnitOfWorkContext _context;

        #endregion

        public GenericRepository(IUnitOfWorkContext context)
        {
            _context = context;
        }

        #region Métodos Públicos

        public async Task<List<T>> ObterTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<(IEnumerable<T> Items, int Total)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string orderByProperty = "",
        bool isDescending = false,
        Expression<Func<T, bool>> filter = null)
        {
            var query = Query();

            // Aplica filtro se fornecido
            if (filter != null)
                query = query.Where(filter);

            // Obtém o total antes da paginação
            var total = await query.CountAsync();

            // Aplica ordenação se propriedade fornecida
            if (!string.IsNullOrWhiteSpace(orderByProperty))
                query = query.OrderByDynamic(orderByProperty, isDescending);

            // Aplica paginação
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
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

        public async Task<List<T>> ObterTodosAsync(int inicioRegistros, int maximoRegistros, string propriedadeOrdenar, bool decrescente = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<T>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(propriedadeOrdenar))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, propriedadeOrdenar);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

                query = decrescente ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
            }

            return await query.Skip(inicioRegistros).Take(maximoRegistros).ToListAsync(cancellationToken);
        }

        public async Task<T> ObterPorIdAsync(long id, bool auditado = false, CancellationToken cancellationToken = default)
        {
            var registro = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (auditado)
                registro?.Initialize();

            return registro;
        }

        public async Task AdicionarAsync(T entidade, AuditadoDto auditado = null, CancellationToken cancellationToken = default)
        {
            await AddAsync(entidade, cancellationToken);

            if (auditado != null)
                AuditarAsync(auditado, entidade, null, AcaoBancoDadosEnum.Insert);
        }

        public async Task AdicionarEmLoteAsync(List<T> listaEntidades, CancellationToken cancellationToken = default)
        {
            await AddManyAsync(listaEntidades, cancellationToken);
        }

        public async Task AtualizarAsync(T entidade, AuditadoDto auditado = null)
        {
            Update(entidade);

            if (auditado != null)
                AuditarAsync(auditado, entidade, entidade.GetChanges(), AcaoBancoDadosEnum.Update);

            await Task.CompletedTask;
        }

        public async Task AtualizarEmLoteAsync(List<T> listaEntidades)
        {
            UpdateMany(listaEntidades);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(T entidade)
        {
            Remove(entidade);
            await Task.CompletedTask;
        }

        public async Task RemoverEmLoteAsync(List<T> listaEntidades)
        {
            RemoveLote(listaEntidades);
            await Task.CompletedTask;
        }

        public IQueryable<T> Query() => _context.Set<T>().AsQueryable();

        public IQueryable<T> OrderByDynamic(string propertyName, bool descending) => _context.Set<T>().OrderByDynamic(propertyName, descending);

        public async Task<int> ContarTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().CountAsync(cancellationToken);
        }

        #endregion Métodos Públicos

        #region Métodos Privados

        private async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        private async Task AddManyAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        private void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.GetEntry(entity).State = EntityState.Modified;
        }

        private void UpdateMany(List<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
            _context.GetEntry(entities).State = EntityState.Modified;
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

        private void AuditarAsync(AuditadoDto auditado, T entidade, List<HistoricoPropriedade> alteracoes, AcaoBancoDadosEnum acao, string descricaoAcao = "")
        {
            if (_context.PendingEntities == null)
                return;

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