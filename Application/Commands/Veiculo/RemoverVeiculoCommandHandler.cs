using Application.Common;
using Dominio.Interfaces.Infra.Data;
using VeiculoEntity = Dominio.Entidades.Veiculos.Veiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo;

public class RemoverVeiculoCommandHandler : IRequestHandler<RemoverVeiculoCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<VeiculoEntity> _veiculoRepository;
    private readonly ILogger<RemoverVeiculoCommandHandler> _logger;

    public RemoverVeiculoCommandHandler(
        IGenericRepository<VeiculoEntity> veiculoRepository,
        ILogger<RemoverVeiculoCommandHandler> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverVeiculoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando remoção do veículo {Id}", request.Id);

            var veiculo = await _veiculoRepository.ObterPorIdAsync(request.Id);
            if (veiculo == null)
                return BaseResponse<bool>.Erro($"Veículo com ID {request.Id} não encontrado");

            await _veiculoRepository.RemoverAsync(veiculo);

            _logger.LogInformation("Veículo {Id} removido com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Veículo removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover veículo {Id}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao remover veículo", new List<string> { ex.Message });
        }
    }
} 
