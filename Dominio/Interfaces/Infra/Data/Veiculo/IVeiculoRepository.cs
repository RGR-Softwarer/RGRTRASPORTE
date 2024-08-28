using Dominio.Dtos.Auditoria;

namespace Dominio.Interfaces.Infra.Data
{
    public interface IVeiculoRepository
    {
        IQueryable<Dominio.Entidades.Veiculo.Veiculo> Query();
        Task<List<Entidades.Veiculo.Veiculo>> ObterTodosAsync();
        Task<Entidades.Veiculo.Veiculo> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<Entidades.Veiculo.Veiculo> listaEntidades);
        void Remover(Entidades.Veiculo.Veiculo entidade);
        void RemoverEmLoteAsync(List<Entidades.Veiculo.Veiculo> listaEntidades);
        void Atualizar(Entidades.Veiculo.Veiculo entidade, AuditadoDto auditado = null);
        void AtualizarEmLoteAsync(List<Entidades.Veiculo.Veiculo> listaEntidades);
        Task AdicionarAsync(Entidades.Veiculo.Veiculo entidade, AuditadoDto auditado = null);
    }
}
