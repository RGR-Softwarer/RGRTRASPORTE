using Application.Common;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class EditarModeloVeicularCommandHandler : IRequestHandler<EditarModeloVeicularCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
    private readonly ILogger<EditarModeloVeicularCommandHandler> _logger;

    public EditarModeloVeicularCommandHandler(
        IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
        ILogger<EditarModeloVeicularCommandHandler> logger)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(EditarModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição do modelo veicular {Id}", request.Id);

            var modeloVeicular = await _modeloVeicularRepository.ObterPorIdAsync(request.Id);
            if (modeloVeicular == null)
                return BaseResponse<bool>.Erro("Modelo veicular não encontrado");

            modeloVeicular.Atualizar(
                request.Descricao,
                request.Tipo,
                request.QuantidadeAssento,
                request.QuantidadeEixo,
                request.CapacidadeMaxima,
                request.PassageirosEmPe,
                request.PossuiBanheiro,
                request.PossuiClimatizador,
                request.Situacao);

            await _modeloVeicularRepository.AtualizarAsync(modeloVeicular);

            _logger.LogInformation("Modelo veicular {Id} atualizado com sucesso", request.Id);

            return BaseResponse<bool>.Ok(true, "Modelo veicular atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao editar modelo veicular {Id}", request.Id);
            return BaseResponse<bool>.Erro("Erro ao editar modelo veicular", new List<string> { ex.Message });
        }
    }
} 