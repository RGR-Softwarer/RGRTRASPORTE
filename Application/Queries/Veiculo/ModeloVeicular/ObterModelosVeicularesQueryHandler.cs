using Application.Common;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Veiculo.ModeloVeicular
{
    public class ObterModelosVeicularesQueryHandler : IRequestHandler<ObterModelosVeicularesQuery, BaseResponse<IEnumerable<ModeloVeicularDto>>>
    {
        private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
        private readonly ILogger<ObterModelosVeicularesQueryHandler> _logger;

        public ObterModelosVeicularesQueryHandler(
            IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
            ILogger<ObterModelosVeicularesQueryHandler> logger)
        {
            _modeloVeicularRepository = modeloVeicularRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<ModeloVeicularDto>>> Handle(ObterModelosVeicularesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando modelos veiculares com filtros - DescricaoFiltro: {DescricaoFiltro}, AtivoFiltro: {AtivoFiltro}", 
                    request.DescricaoFiltro, request.AtivoFiltro);

                var modelos = await _modeloVeicularRepository.ObterTodosAsync(cancellationToken);

                var modelosFiltrados = modelos.AsQueryable();

                if (!string.IsNullOrEmpty(request.DescricaoFiltro))
                    modelosFiltrados = modelosFiltrados.Where(m => m.Descricao.ToLower().Contains(request.DescricaoFiltro.ToLower()));

                if (request.AtivoFiltro.HasValue)
                    modelosFiltrados = modelosFiltrados.Where(m => m.Situacao == request.AtivoFiltro.Value);

                if (request.TipoFiltro.HasValue)
                    modelosFiltrados = modelosFiltrados.Where(m => m.Tipo == request.TipoFiltro.Value);

                var modelosDto = modelosFiltrados.Select(m => new ModeloVeicularDto
                {
                    Id = m.Id,
                    DescricaoModelo = m.Descricao,
                    Tipo = m.Tipo,
                    Situacao = m.Situacao,
                    QuantidadeAssento = m.QuantidadeAssento,
                    QuantidadeEixo = m.QuantidadeEixo,
                    CapacidadeMaxima = m.CapacidadeMaxima,
                    PassageirosEmPe = m.PassageirosEmPe,
                    PossuiBanheiro = m.PossuiBanheiro,
                    PossuiClimatizador = m.PossuiClimatizador
                }).ToList();

                _logger.LogInformation("Modelos veiculares encontrados com sucesso. Total: {Total}", modelosDto.Count);

                return BaseResponse<IEnumerable<ModeloVeicularDto>>.Ok(modelosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar modelos veiculares");
                return BaseResponse<IEnumerable<ModeloVeicularDto>>.Erro("Erro ao buscar modelos veiculares", new List<string> { ex.Message });
            }
        }
    }
} 
