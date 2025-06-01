using Dominio.Interfaces.Service.Passageiros;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Entidades.Pessoas.Passageiros;

namespace Service.Services.Passageiros;

public class PassageiroService : IPassageiroService
{
    private readonly IPassageiroRepository _passageiroRepository;

    public PassageiroService(IPassageiroRepository passageiroRepository)
    {
        _passageiroRepository = passageiroRepository;
    }

    public async Task<Passageiro> ObterPorIdAsync(long id)
    {
        return await _passageiroRepository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<Passageiro>> ObterTodosAsync()
    {
        return await _passageiroRepository.ObterTodosAsync();
    }

    public async Task<Passageiro> CriarAsync(Passageiro passageiro)
    {
        await _passageiroRepository.AdicionarAsync(passageiro);
        return passageiro;
    }

    public async Task<Passageiro> AtualizarAsync(Passageiro passageiro)
    {
        await _passageiroRepository.AtualizarAsync(passageiro);
        return passageiro;
    }

    public async Task<bool> RemoverAsync(long id)
    {
        var passageiro = await _passageiroRepository.ObterPorIdAsync(id);
        if (passageiro == null) return false;
        
        await _passageiroRepository.RemoverAsync(passageiro);
        return true;
    }
} 