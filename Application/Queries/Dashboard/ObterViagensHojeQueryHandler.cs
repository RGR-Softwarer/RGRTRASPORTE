using Application.Common;
using Application.Queries.Viagem;
using Application.Queries.Viagem.Mappers;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Dashboard;

public class ObterViagensHojeQueryHandler : IRequestHandler<ObterViagensHojeQuery, BaseResponse<IEnumerable<Application.Queries.Viagem.Models.ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterViagensHojeQueryHandler> _logger;

    public ObterViagensHojeQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterViagensHojeQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<Application.Queries.Viagem.Models.ViagemDto>>> Handle(ObterViagensHojeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando viagens de hoje");

            var hoje = DateTime.Today;
            var amanha = hoje.AddDays(1);

            var viagens = await _viagemRepository.ObterViagensPorDataAsync(hoje, amanha);
            var viagensDto = viagens.ToDto();

            _logger.LogInformation("Viagens de hoje obtidas com sucesso. Total: {Total}", viagensDto.Count());

            return BaseResponse<IEnumerable<Application.Queries.Viagem.Models.ViagemDto>>.Ok(viagensDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar viagens de hoje");
            return BaseResponse<IEnumerable<Application.Queries.Viagem.Models.ViagemDto>>.Erro("Erro ao buscar viagens de hoje", new List<string> { ex.Message });
        }
    }
}


