using Application.Common;
using Application.Queries.Passageiro.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Passageiro;

public class ObterPassageiroPorIdQueryHandler : IRequestHandler<ObterPassageiroPorIdQuery, BaseResponse<PassageiroDto>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObterPassageiroPorIdQueryHandler> _logger;

    public ObterPassageiroPorIdQueryHandler(
        IPassageiroRepository passageiroRepository,
        IMapper mapper,
        ILogger<ObterPassageiroPorIdQueryHandler> logger)
    {
        _passageiroRepository = passageiroRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<PassageiroDto>> Handle(ObterPassageiroPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando passageiro com ID {PassageiroId}", request.Id);

            var passageiro = await _passageiroRepository.ObterPorIdAsync(request.Id);
            
            if (passageiro == null)
            {
                _logger.LogWarning("Passageiro não encontrado com ID {PassageiroId}", request.Id);
                return BaseResponse<PassageiroDto>.Erro("Passageiro não encontrado");
            }

            var passageiroDto = _mapper.Map<PassageiroDto>(passageiro);
            
            _logger.LogInformation("Passageiro encontrado com sucesso. ID: {PassageiroId}", passageiro.Id);
            
            return BaseResponse<PassageiroDto>.Ok(passageiroDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar passageiro");
            return BaseResponse<PassageiroDto>.Erro("Erro ao buscar passageiro", new List<string> { ex.Message });
        }
    }
} 
