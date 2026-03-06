using Application.Common;
using Application.Queries.Veiculo.Models;
using Dominio.Enums.Viagens;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Infra.Data.Viagens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Dashboard;

public class ObterVeiculosEmViagemQueryHandler : IRequestHandler<ObterVeiculosEmViagemQuery, BaseResponse<IEnumerable<VeiculoDto>>>
{
    private readonly IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> _veiculoRepository;
    private readonly IViagemRepository _viagemRepository;
    private readonly ILogger<ObterVeiculosEmViagemQueryHandler> _logger;

    public ObterVeiculosEmViagemQueryHandler(
        IGenericRepository<Dominio.Entidades.Veiculos.Veiculo> veiculoRepository,
        IViagemRepository viagemRepository,
        ILogger<ObterVeiculosEmViagemQueryHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _viagemRepository = viagemRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<VeiculoDto>>> Handle(ObterVeiculosEmViagemQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando veículos em viagem");

            // Buscar viagens em andamento
            var viagensEmAndamento = await _viagemRepository.ObterViagensEmAndamentoAsync();
            var veiculoIdsEmViagem = viagensEmAndamento.Select(v => v.VeiculoId).Distinct().ToList();

            // Buscar veículos que estão em viagem
            var todosVeiculos = await _veiculoRepository.ObterTodosAsync();
            var veiculosEmViagem = todosVeiculos.Where(v => veiculoIdsEmViagem.Contains(v.Id) && v.Situacao).ToList();

            var veiculosDto = veiculosEmViagem.Select(v => new VeiculoDto
            {
                Id = v.Id,
                Placa = v.Placa?.Numero ?? string.Empty,
                PlacaFormatada = v.PlacaFormatada ?? string.Empty,
                Modelo = v.ModeloVeiculo?.Descricao ?? v.Modelo ?? string.Empty,
                Marca = v.Marca ?? string.Empty,
                NumeroChassi = v.NumeroChassi ?? string.Empty,
                AnoModelo = v.AnoModelo,
                AnoFabricacao = v.AnoFabricacao,
                Cor = v.Cor ?? string.Empty,
                Renavam = v.Renavam ?? string.Empty,
                VencimentoLicenciamento = v.VencimentoLicenciamento,
                TipoCombustivel = v.TipoCombustivel,
                TipoCombustivelDescricao = v.TipoCombustivel.ToString(),
                Status = v.Status,
                StatusDescricao = v.Status.ToString(),
                Observacao = v.Observacao ?? string.Empty,
                ModeloVeiculoId = v.ModeloVeiculoId,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                Capacidade = v.ModeloVeiculo?.CapacidadeMaxima ?? 0,
                Ativo = v.Situacao
            });

            _logger.LogInformation("Veículos em viagem obtidos com sucesso. Total: {Total}", veiculosEmViagem.Count);

            return BaseResponse<IEnumerable<VeiculoDto>>.Ok(veiculosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar veículos em viagem");
            return BaseResponse<IEnumerable<VeiculoDto>>.Erro("Erro ao buscar veículos em viagem", new List<string> { ex.Message });
        }
    }
}

