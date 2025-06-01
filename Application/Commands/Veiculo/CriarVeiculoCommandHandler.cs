using Application.Common;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo;

public class CriarVeiculoCommandHandler : IRequestHandler<CriarVeiculoCommand, BaseResponse<long>>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly ILogger<CriarVeiculoCommandHandler> _logger;

    public CriarVeiculoCommandHandler(
        IVeiculoRepository veiculoRepository,
        ILogger<CriarVeiculoCommandHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<long>> Handle(CriarVeiculoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação de veículo com placa {Placa}", request.Placa);

            var veiculo = new Dominio.Entidades.Veiculos.Veiculo(
                request.Placa,
                request.Modelo,
                request.Marca,
                request.NumeroChassi,
                request.AnoModelo,
                request.AnoFabricacao,
                request.Cor,
                request.Renavam,
                request.VencimentoLicenciamento,
                request.TipoCombustivel,
                request.Status,
                request.Observacao,
                request.ModeloVeiculoId);

            await _veiculoRepository.AdicionarAsync(veiculo, cancellationToken: cancellationToken);

            _logger.LogInformation("Veículo criado com sucesso. ID: {VeiculoId}", veiculo.Id);

            return BaseResponse<long>.Ok(veiculo.Id, "Veículo criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar veículo");
            return BaseResponse<long>.Erro("Erro ao criar veículo", new List<string> { ex.Message });
        }
    }
} 