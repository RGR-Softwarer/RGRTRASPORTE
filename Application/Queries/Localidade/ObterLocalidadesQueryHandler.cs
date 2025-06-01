using Application.Common;
using Application.Queries.Localidade.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Localidades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Localidade;

public class ObterLocalidadesQueryHandler : IRequestHandler<ObterLocalidadesQuery, BaseResponse<IEnumerable<LocalidadeDto>>>
{
    private readonly ILocalidadeRepository _localidadeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObterLocalidadesQueryHandler> _logger;

    public ObterLocalidadesQueryHandler(
        ILocalidadeRepository localidadeRepository,
        IMapper mapper,
        ILogger<ObterLocalidadesQueryHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<LocalidadeDto>>> Handle(ObterLocalidadesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando localidades com filtros - Nome: {Nome}, Estado: {Estado}, Ativo: {Ativo}", 
                request.Nome, request.Estado, request.Ativo);

            var localidades = await _localidadeRepository.ObterTodosAsync();

            // Aplicar filtros se fornecidos
            if (!string.IsNullOrEmpty(request.Nome))
            {
                localidades = localidades.Where(l => l.Nome.Contains(request.Nome, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(request.Estado))
            {
                localidades = localidades.Where(l => l.Estado.Contains(request.Estado, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (request.Ativo.HasValue)
            {
                localidades = localidades.Where(l => l.Ativo == request.Ativo.Value).ToList();
            }

            var localidadesDto = _mapper.Map<IEnumerable<LocalidadeDto>>(localidades);

            _logger.LogInformation("Localidades encontradas com sucesso. Total: {Total}", localidadesDto.Count());

            return BaseResponse<IEnumerable<LocalidadeDto>>.Ok(localidadesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar localidades");
            return BaseResponse<IEnumerable<LocalidadeDto>>.Erro("Erro ao buscar localidades", new List<string> { ex.Message });
        }
    }
} 