using Application.Common;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo;

public class AdicionarVeiculosEmLoteCommandHandler : IRequestHandler<AdicionarVeiculosEmLoteCommand, BaseResponse<IEnumerable<long>>>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly ILogger<AdicionarVeiculosEmLoteCommandHandler> _logger;

    public AdicionarVeiculosEmLoteCommandHandler(
        IVeiculoRepository veiculoRepository,
        ILogger<AdicionarVeiculosEmLoteCommandHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<long>>> Handle(AdicionarVeiculosEmLoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando adição em lote de {Quantidade} veículos", request.Veiculos.Count());

            var veiculos = request.Veiculos.Select(v => new Dominio.Entidades.Veiculos.Veiculo(
                v.Placa,
                v.Modelo,
                v.Marca,
                v.NumeroChassi,
                v.AnoModelo,
                v.AnoFabricacao,
                v.Cor,
                v.Renavam,
                v.VencimentoLicenciamento,
                v.TipoCombustivel,
                v.Status,
                v.Observacao,
                v.ModeloVeiculoId));

            await _veiculoRepository.AdicionarEmLoteAsync(veiculos.ToList());

            var ids = veiculos.Select(v => v.Id);

            _logger.LogInformation("Veículos adicionados com sucesso");

            return BaseResponse<IEnumerable<long>>.Ok(ids, "Veículos adicionados com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar veículos em lote");
            return BaseResponse<IEnumerable<long>>.Erro("Erro ao adicionar veículos em lote", new List<string> { ex.Message });
        }
    }
} 