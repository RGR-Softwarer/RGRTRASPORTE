using Dominio.Dtos.Auditoria;
using Dominio.Entidades.Veiculo;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IVeiculoRepository
    {
        IQueryable<Veiculo> Query();
        Task<List<Veiculo>> ObterTodosAsync();
        Task<Veiculo> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<Veiculo> listaEntidades);
        void Remover(Veiculo entidade);
        void RemoverEmLoteAsync(List<Veiculo> listaEntidades);
        void Atualizar(Veiculo entidade, AuditadoDto auditado = null);
        void AtualizarEmLoteAsync(List<Veiculo> listaEntidades);
        Task AdicionarAsync(Veiculo entidade, AuditadoDto auditado = null);
    }
}
