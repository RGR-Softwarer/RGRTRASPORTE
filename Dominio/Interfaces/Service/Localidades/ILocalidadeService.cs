using Dominio.Dtos.Localidades;

namespace Dominio.Interfaces.Service.Localidades
{
    public interface ILocalidadeService
    {
        Task<IEnumerable<LocalidadeDto>> ObterTodosAsync();
        Task<LocalidadeDto> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarAsync(LocalidadeDto dto);
        Task EditarAsync(LocalidadeDto dto);
        Task RemoverAsync(long id);
    }
}