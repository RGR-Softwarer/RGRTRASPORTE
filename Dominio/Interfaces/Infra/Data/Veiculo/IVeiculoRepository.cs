using Dominio.Dtos.Auditoria;

namespace Dominio.Interfaces.Infra.Data.Veiculo
{
    public interface IVeiculoRepository
    {
        IQueryable<Entidades.Veiculos.Veiculo> Query();
        Task<List<Entidades.Veiculos.Veiculo>> ObterTodosAsync();
        Task<Entidades.Veiculos.Veiculo> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarEmLoteAsync(List<Entidades.Veiculos.Veiculo> listaEntidades);
        Task RemoverAsync(Entidades.Veiculos.Veiculo entidade);
        Task RemoverEmLoteAsync(List<Entidades.Veiculos.Veiculo> listaEntidades);
        Task AtualizarAsync(Entidades.Veiculos.Veiculo entidade, AuditadoDto auditado = null);
        Task AtualizarEmLoteAsync(List<Entidades.Veiculos.Veiculo> listaEntidades);
        Task AdicionarAsync(Entidades.Veiculos.Veiculo entidade, AuditadoDto auditado = null);
    }
}
