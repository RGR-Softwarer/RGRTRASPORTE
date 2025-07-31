using Application.Common;
using Application.Queries.Viagem.Models;
using Dominio.Interfaces.Infra.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using ViagemPosicaoEntity = Dominio.Entidades.Viagens.ViagemPosicao;

namespace Application.Queries.Viagem.ViagemPosicao
{
    public class ObterViagemPosicoesQueryHandler : IRequestHandler<ObterViagemPosicoesQuery, BaseResponse<IEnumerable<ViagemPosicaoDto>>>
    {
        private readonly IGenericRepository<ViagemPosicaoEntity> _viagemPosicaoRepository;
        private readonly ILogger<ObterViagemPosicoesQueryHandler> _logger;

        public ObterViagemPosicoesQueryHandler(
            IGenericRepository<ViagemPosicaoEntity> viagemPosicaoRepository,
            ILogger<ObterViagemPosicoesQueryHandler> logger)
        {
            _viagemPosicaoRepository = viagemPosicaoRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<ViagemPosicaoDto>>> Handle(ObterViagemPosicoesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando posições da viagem - ViagemId: {ViagemId}, DataInicio: {DataInicio}, DataFim: {DataFim}", 
                    request.ViagemId, request.DataInicio, request.DataFim);

                var posicoes = await _viagemPosicaoRepository.ObterTodosAsync(cancellationToken);

                var posicoesFiltradas = posicoes.AsQueryable();

                if (request.ViagemId > 0)
                    posicoesFiltradas = posicoesFiltradas.Where(p => p.ViagemId == request.ViagemId);

                if (request.DataInicio.HasValue)
                    posicoesFiltradas = posicoesFiltradas.Where(p => p.DataHora >= request.DataInicio.Value);

                if (request.DataFim.HasValue)
                    posicoesFiltradas = posicoesFiltradas.Where(p => p.DataHora <= request.DataFim.Value);

                var posicoesDto = posicoesFiltradas.Select(p => new ViagemPosicaoDto
                {
                    Id = p.Id,
                    ViagemId = p.ViagemId,
                    Latitude = p.Latitude.ToString(),
                    Longitude = p.Longitude.ToString(),
                    DataHora = p.DataHora
                }).ToList();

                _logger.LogInformation("Posições da viagem encontradas com sucesso. Total: {Total}", posicoesDto.Count);

                return BaseResponse<IEnumerable<ViagemPosicaoDto>>.Ok(posicoesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar posições da viagem");
                return BaseResponse<IEnumerable<ViagemPosicaoDto>>.Erro("Erro ao buscar posições da viagem", new List<string> { ex.Message });
            }
        }
    }
} 
