using Application.Common;
using Application.Queries.Viagem.Models;
using Application.Queries.Viagem.Mappers;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Viagem;

public class ObterViagensQueryHandler : IRequestHandler<ObterViagensQuery, BaseResponse<IEnumerable<ViagemDto>>>
{
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterViagensQueryHandler> _logger;

    public ObterViagensQueryHandler(
        IViagemRepository viagemRepository,
        ILogger<ObterViagensQueryHandler> logger)
    {
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ViagemDto>>> Handle(ObterViagensQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando viagens com filtros: DataInicio: {DataInicio}, DataFim: {DataFim}, LocalidadeOrigemId: {LocalidadeOrigemId}, LocalidadeDestinoId: {LocalidadeDestinoId}",
                request.DataInicio, request.DataFim, request.LocalidadeOrigemId, request.LocalidadeDestinoId);

            IEnumerable<Dominio.Entidades.Viagens.Viagem> viagens;

            if (request.DataInicio.HasValue && request.DataFim.HasValue)
            {
                viagens = await _viagemRepository.ObterViagensPorDataAsync(
                    request.DataInicio.Value,
                    request.DataFim.Value);
            }
            else if (request.LocalidadeOrigemId.HasValue)
            {
                viagens = await _viagemRepository.ObterViagensPorLocalidadeAsync(
                    request.LocalidadeOrigemId.Value, true);
            }
            else if (request.LocalidadeDestinoId.HasValue)
            {
                viagens = await _viagemRepository.ObterViagensPorLocalidadeAsync(
                    request.LocalidadeDestinoId.Value, false);
            }
            else
            {
                viagens = await _viagemRepository.ObterTodosAsync();
            }

            // Mapeamento manual explícito - alinhado com CQRS
            var viagensDto = viagens.ToDto();

            _logger.LogInformation("Viagens encontradas com sucesso. Total: {Total}", viagensDto.Count());

            return BaseResponse<IEnumerable<ViagemDto>>.Ok(viagensDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar viagens");
            return BaseResponse<IEnumerable<ViagemDto>>.Erro("Erro ao buscar viagens", new List<string> { ex.Message });
        }
    }
} 
