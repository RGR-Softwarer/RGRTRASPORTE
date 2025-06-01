using Application.Common;
using Dominio.Interfaces.Infra.Data.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo;

public class EditarVeiculoCommandHandler : IRequestHandler<EditarVeiculoCommand, BaseResponse<bool>>
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly ILogger<EditarVeiculoCommandHandler> _logger;

    public EditarVeiculoCommandHandler(
        IVeiculoRepository veiculoRepository,
        ILogger<EditarVeiculoCommandHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarVeiculoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição do veículo {Id}", request.Id);

            var veiculo = await _veiculoRepository.ObterPorIdAsync(request.Id);
            if (veiculo == null)
                return BaseResponse<bool>.Erro($"Veículo com ID {request.Id} não encontrado");

            var veiculoAtualizado = new Dominio.Entidades.Veiculos.Veiculo(
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

            veiculoAtualizado.Id = request.Id;

            await _veiculoRepository.AtualizarAsync(veiculoAtualizado);

            _logger.LogInformation("Veículo {Id} atualizado com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Veículo atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar veículo {Id}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao atualizar veículo", new List<string> { ex.Message });
        }
    }
} 