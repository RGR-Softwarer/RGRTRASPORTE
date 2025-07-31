using Application.Common;
using Application.Queries.Passageiro.Models;
using AutoMapper;
using Dominio.Interfaces.Infra.Data.Passageiros;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Passageiro;

public class ListarPassageirosQueryHandler : IRequestHandler<ListarPassageirosQuery, BaseResponse<IEnumerable<PassageiroDto>>>
{
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListarPassageirosQueryHandler> _logger;

    public ListarPassageirosQueryHandler(
        IPassageiroRepository passageiroRepository,
        IMapper mapper,
        ILogger<ListarPassageirosQueryHandler> logger)
    {
        _passageiroRepository = passageiroRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<PassageiroDto>>> Handle(ListarPassageirosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Listando passageiros");

            var passageiros = await _passageiroRepository.ObterTodosAsync();
            var passageirosDto = _mapper.Map<IEnumerable<PassageiroDto>>(passageiros);

            _logger.LogInformation("Passageiros listados com sucesso. Total: {Total}", passageirosDto.Count());

            return BaseResponse<IEnumerable<PassageiroDto>>.Ok(passageirosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar passageiros");
            return BaseResponse<IEnumerable<PassageiroDto>>.Erro("Erro ao listar passageiros", new List<string> { ex.Message });
        }
    }
} 
