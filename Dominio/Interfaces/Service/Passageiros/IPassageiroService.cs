using Dominio.Dtos.Pessoas.Passageiros;

namespace Dominio.Interfaces.Service.Passageiros
{
    public interface IPassageiroService
    {
        Task<IEnumerable<PassageiroDto>> ObterTodosAsync();
        Task<PassageiroDto> ObterPorIdAsync(long id, bool auditado = false);
        Task AdicionarAsync(PassageiroDto dto);
        Task EditarAsync(PassageiroDto dto);
        Task RemoverAsync(long id);
    }
}