using Dominio.Dtos.Viagens;

namespace Dominio.Interfaces.Service.Viagens;

public interface IViagemService
{
    Task<ViagemDto> ObterPorIdAsync(long id);
    Task<IEnumerable<ViagemDto>> ObterTodosAsync();
    Task<ViagemDto> CriarAsync(ViagemDto viagemDto);
    Task<ViagemDto> AtualizarAsync(ViagemDto viagemDto);
    Task<bool> RemoverAsync(long id);
    Task<bool> IniciarViagemAsync(long id, string usuarioOperacao);
    Task<bool> FinalizarViagemAsync(long id, string usuarioOperacao);
} 