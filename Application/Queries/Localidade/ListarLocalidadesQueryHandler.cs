using Application.Common;
using Application.Queries.Localidade.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Localidades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Localidade;

public class ListarLocalidadesQueryHandler : IRequestHandler<ListarLocalidadesQuery, BaseResponse<IEnumerable<LocalidadeDto>>>
{
    private readonly ILocalidadeRepository _localidadeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListarLocalidadesQueryHandler> _logger;

    public ListarLocalidadesQueryHandler(
        ILocalidadeRepository localidadeRepository,
        IMapper mapper,
        ILogger<ListarLocalidadesQueryHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<LocalidadeDto>>> Handle(ListarLocalidadesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Listando localidades");

            var localidades = await _localidadeRepository.ObterTodosAsync();
            var localidadesDto = _mapper.Map<IEnumerable<LocalidadeDto>>(localidades);

            _logger.LogInformation("Localidades listadas com sucesso. Total: {Total}", localidadesDto.Count());

            return BaseResponse<IEnumerable<LocalidadeDto>>.Ok(localidadesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar localidades");
            return BaseResponse<IEnumerable<LocalidadeDto>>.Erro("Erro ao listar localidades", new List<string> { ex.Message });
        }
    }
} 