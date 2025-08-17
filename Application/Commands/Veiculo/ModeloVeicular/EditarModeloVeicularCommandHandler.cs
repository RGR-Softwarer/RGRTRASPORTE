using Application.Common;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;
using Dominio.Interfaces;
using Dominio.Services;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class EditarModeloVeicularCommandHandler : IRequestHandler<EditarModeloVeicularCommand, BaseResponse<bool>>
{
    private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
    private readonly ILogger<EditarModeloVeicularCommandHandler> _logger;
    private readonly INotificationContext _notificationContext;

    public EditarModeloVeicularCommandHandler(
        IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
        ILogger<EditarModeloVeicularCommandHandler> logger,
        INotificationContext notificationContext)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
        _notificationContext = notificationContext;
    }

    public async Task<BaseResponse<bool>> Handle(EditarModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando edição do modelo veicular {Id}", request.Id);

            var modeloVeicular = await _modeloVeicularRepository.ObterPorIdAsync(request.Id);
            if (modeloVeicular == null)
                return BaseResponse<bool>.Erro("Modelo veicular não encontrado");

            // Usar Domain Service para validar atualização
            var validationService = new ModeloVeicularValidationService();
            var valido = validationService.ValidarAtualizacao(
                modeloVeicular,
                request.Descricao,
                request.Tipo,
                request.QuantidadeAssento,
                request.QuantidadeEixo,
                request.CapacidadeMaxima,
                request.PassageirosEmPe,
                request.PossuiBanheiro,
                request.PossuiClimatizador,
                _notificationContext);

            if (!valido)
            {
                _logger.LogWarning("Falha na validação da atualização do modelo veicular {Id}. Total de erros: {Count}", 
                    request.Id, _notificationContext.GetNotificationCount());
                return BaseResponse<bool>.Erro("Dados inválidos para atualização do modelo veicular", new List<string> { "Falha na validação dos dados" });
            }

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
