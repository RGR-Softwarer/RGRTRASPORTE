using Dominio.Dtos.Viagens;
using Dominio.Interfaces.Service.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using AutoMapper;

namespace Service.Services.Viagens;

public class ViagemService : IViagemService
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IMapper _mapper;

    public ViagemService(IViagemRepository viagemRepository, IMapper mapper)
    {
        _viagemRepository = viagemRepository;
        _mapper = mapper;
    }

    public async Task<ViagemDto> ObterPorIdAsync(long id)
    {
        var viagem = await _viagemRepository.ObterViagemCompletaPorIdAsync(id);
        return _mapper.Map<ViagemDto>(viagem);
    }

    public async Task<IEnumerable<ViagemDto>> ObterTodosAsync()
    {
        var viagens = await _viagemRepository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<ViagemDto>>(viagens);
    }

    public async Task<ViagemDto> CriarAsync(ViagemDto viagemDto)
    {
        var viagem = _mapper.Map<Dominio.Entidades.Viagens.Viagem>(viagemDto);
        await _viagemRepository.AdicionarAsync(viagem);
        return _mapper.Map<ViagemDto>(viagem);
    }

    public async Task<ViagemDto> AtualizarAsync(ViagemDto viagemDto)
    {
        var viagem = _mapper.Map<Dominio.Entidades.Viagens.Viagem>(viagemDto);
        await _viagemRepository.AtualizarAsync(viagem);
        return _mapper.Map<ViagemDto>(viagem);
    }

    public async Task<bool> RemoverAsync(long id)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(id);
        if (viagem == null) return false;
        
        await _viagemRepository.RemoverAsync(viagem);
        return true;
    }

    public async Task<bool> IniciarViagemAsync(long id, string usuarioOperacao)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(id);
        if (viagem == null) return false;

        // Implementar lógica de iniciar viagem
        return true;
    }

    public async Task<bool> FinalizarViagemAsync(long id, string usuarioOperacao)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(id);
        if (viagem == null) return false;

        // Implementar lógica de finalizar viagem
        return true;
    }
} 