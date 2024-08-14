using Dominio.Dtos.Auditoria;
using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Enums;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> ObterPorIdAsync(long id, bool auditado = false)
        {
            var registro = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            if (auditado)
                registro.Initialize();

            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AdicionarAsync(T entidade, AuditadoDto auditado = null)
        {
            await AddAsync(entidade);

            if (auditado != null)
                await AuditarAsync(auditado, entidade, null, AcaoBancoDadosEnum.Insert);
        }

        public async Task AdicionarEmLoteAsync(List<T> listaEntidades)
        {
            await AddManyAsync(listaEntidades);
        }

        public void Atualizar(T entidade, AuditadoDto auditado = null)
        {
            Update(entidade);

            if (auditado != null)
                _ = AuditarAsync(auditado, entidade, entidade.GetChanges(), AcaoBancoDadosEnum.Update);
        }

        public void AtualizarEmLoteAsync(List<T> listaEntidades)
        {
            UpdateMany(listaEntidades);
        }

        public void Remover(T entidade)
        {
            Remove(entidade);
        }

        public void RemoverEmLoteAsync(List<T> listaEntidades)
        {
            RemoveLote(listaEntidades);
        }

        public IQueryable<T> Query() => _context.Set<T>().AsQueryable();

        public async Task<int> ContarTodosAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<List<T>> ObterTodosAsync(int inicioRegistros, int maximoRegistros, string propriedadeOrdenar, bool decrescente = false)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(propriedadeOrdenar))
            {
                query = decrescente
                    ? query.OrderByDescending(x => EF.Property<object>(x, propriedadeOrdenar))
                    : query.OrderBy(x => EF.Property<object>(x, propriedadeOrdenar));
            }

            return await query.Skip(inicioRegistros).Take(maximoRegistros).ToListAsync();
        }

        #endregion Métodos Públicos

        #region Métodos Privados

        private async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        private async Task AddManyAsync(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        private void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void UpdateMany(List<T> entity)
        {
            _context.Set<T>().AttachRange(entity);
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
                Descricao = entidade.DescricaoAuditoria ?? string.Empty,
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