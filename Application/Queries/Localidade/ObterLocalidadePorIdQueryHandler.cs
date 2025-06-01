using Application.Common;
using Application.Queries.Localidade.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Localidades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Localidade;

public class ObterLocalidadePorIdQueryHandler : IRequestHandler<ObterLocalidadePorIdQuery, BaseResponse<LocalidadeDto>>
{
    private readonly ILocalidadeRepository _localidadeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObterLocalidadePorIdQueryHandler> _logger;

    public ObterLocalidadePorIdQueryHandler(
        ILocalidadeRepository localidadeRepository,
        IMapper mapper,
        ILogger<ObterLocalidadePorIdQueryHandler> logger)
    {
        _localidadeRepository = localidadeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<LocalidadeDto>> Handle(ObterLocalidadePorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando localidade com ID {LocalidadeId}", request.Id);

            var localidade = await _localidadeRepository.ObterPorIdAsync(request.Id);
            
            if (localidade == null)
            {
                _logger.LogWarning("Localidade não encontrada com ID {LocalidadeId}", request.Id);
                return BaseResponse<LocalidadeDto>.Erro("Localidade não encontrada");
            }

            var localidadeDto = _mapper.Map<LocalidadeDto>(localidade);
            
            _logger.LogInformation("Localidade encontrada com sucesso. ID: {LocalidadeId}", localidade.Id);
            
            return BaseResponse<LocalidadeDto>.Ok(localidadeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar localidade");
            return BaseResponse<LocalidadeDto>.Erro("Erro ao buscar localidade", new List<string> { ex.Message });
        }
    }
} 