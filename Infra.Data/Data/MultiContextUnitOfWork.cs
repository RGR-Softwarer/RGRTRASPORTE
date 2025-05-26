using Dominio.Entidades;
using Dominio.Entidades.Auditoria;
using Dominio.Interfaces.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Transactions;

namespace Infra.Data.Data
{
    public class MultiContextUnitOfWork : IUnitOfWork
    {
        private readonly IEnumerable<IUnitOfWorkContext> _dbContexts;
        private TransactionScope _transactionScope;
        private bool _disposed;

        public MultiContextUnitOfWork(IEnumerable<IUnitOfWorkContext> dbContexts)
        {
            _dbContexts = dbContexts ?? throw new ArgumentNullException(nameof(dbContexts));
            if (!_dbContexts.Any())
                throw new ArgumentException("Pelo menos um contexto deve ser fornecido", nameof(dbContexts));
        }

        public async Task<int> Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MultiContextUnitOfWork));

            var totalChanges = 0;

            // Configura as opções da transação distribuída
            var options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            // Cria um novo escopo de transação distribuída
            using (_transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                options,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Salva as mudanças em todos os contextos
                    foreach (var context in _dbContexts)
                    {
                        if (context.ChangeTracker.HasChanges())
                        {
                            var changes = await context.SaveChangesAsync();
                            totalChanges += changes;

                            // Processa a auditoria para as entidades pendentes
                            if (context.PendingEntities != null && context.PendingEntities.Any())
                            {
                                foreach (var (entity, historicoObjeto) in context.PendingEntities)
                                {
                                    if (entity != null && historicoObjeto != null)
                                    {
                                        historicoObjeto.CodigoObjeto = entity.Id;
                                        historicoObjeto.DescricaoObjeto = entity.DescricaoAuditoria;
                                        historicoObjeto.Data = DateTime.UtcNow;

                                        context.Set<HistoricoObjeto>().Add(historicoObjeto);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                context.PendingEntities.Clear();
                            }
                        }
                    }

                    // Completa a transação distribuída
                    _transactionScope.Complete();
                }
                catch (Exception)
                {
                    // Em caso de erro, a transação será automaticamente revertida
                    throw;
                }
            }

            return totalChanges;
        }

        public async Task RollBack()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MultiContextUnitOfWork));

            try
            {
                // Descarta o escopo da transação sem chamar Complete()
                _transactionScope?.Dispose();
                _transactionScope = null;

                // Recarrega as entidades modificadas
                var rollbackTasks = _dbContexts
                    .SelectMany(context => context.ChangeTracker.Entries()
                        .Where(e => e.State != EntityState.Unchanged)
                        .Select(entry => entry.ReloadAsync()))
                    .ToList();

                await Task.WhenAll(rollbackTasks);
            }
            catch (Exception)
            {
                // Ignora erros durante o rollback
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transactionScope?.Dispose();
                    _transactionScope = null;

                    foreach (var context in _dbContexts)
                    {
                        context.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~MultiContextUnitOfWork()
        {
            Dispose(false);
        }
    }
} 