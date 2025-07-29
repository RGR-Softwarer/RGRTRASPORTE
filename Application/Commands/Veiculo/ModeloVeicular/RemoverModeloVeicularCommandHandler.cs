using Application.Common;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class RemoverModeloVeicularCommandHandler : IRequestHandler<RemoverModeloVeicularCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
    private readonly ILogger<RemoverModeloVeicularCommandHandler> _logger;

    public RemoverModeloVeicularCommandHandler(
        IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
        ILogger<RemoverModeloVeicularCommandHandler> logger)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(RemoverModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando remoção do modelo veicular {Id}", request.Id);

            var modeloVeicular = await _modeloVeicularRepository.ObterPorIdAsync(request.Id);
            if (modeloVeicular == null)
                return BaseResponse<bool>.Erro("Modelo veicular não encontrado");

            await _modeloVeicularRepository.RemoverAsync(modeloVeicular);

            _logger.LogInformation("Modelo veicular {Id} removido com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Modelo veicular removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover modelo veicular {Id}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao remover modelo veicular", new List<string> { ex.Message });
        }
    }
} 