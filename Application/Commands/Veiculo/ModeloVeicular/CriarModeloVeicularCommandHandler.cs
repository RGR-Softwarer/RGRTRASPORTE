using Application.Common;
using Dominio.Interfaces.Infra.Data;
using ModeloVeicularEntity = Dominio.Entidades.Veiculos.ModeloVeicular;
using MediatR;
using Microsoft.Extensions.Logging;
using Infra.CrossCutting.Handlers.Notifications;

namespace Application.Commands.Veiculo.ModeloVeicular;

public class CriarModeloVeicularCommandHandler : IRequestHandler<CriarModeloVeicularCommand, BaseResponse<long>>
{
    private readonly IGenericRepository<ModeloVeicularEntity> _modeloVeicularRepository;
    private readonly ILogger<CriarModeloVeicularCommandHandler> _logger;
    private readonly INotificationContext _notificationContext;

    public CriarModeloVeicularCommandHandler(
        IGenericRepository<ModeloVeicularEntity> modeloVeicularRepository,
        ILogger<CriarModeloVeicularCommandHandler> logger,
        INotificationContext notificationContext)
    {
        _modeloVeicularRepository = modeloVeicularRepository;
        _logger = logger;
        _notificationContext = notificationContext;
    }

    public async Task<BaseResponse<long>> Handle(CriarModeloVeicularCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação do modelo veicular {Nome}", request.Descricao);

            // Criar adapter para converter interface
            var domainNotificationContext = new DomainNotificationContextAdapter(_notificationContext);

            // Usar Factory Method com validação
            var (modeloVeicular, sucesso) = ModeloVeicularEntity.CriarModeloVeicularComValidacao(
                request.Descricao,
                request.Tipo,
                request.QuantidadeAssento,
                request.QuantidadeEixo,
                request.CapacidadeMaxima,
                request.PassageirosEmPe,
                request.PossuiBanheiro,
                request.PossuiClimatizador,
                domainNotificationContext);

            if (!sucesso || modeloVeicular == null)
            {
                _logger.LogWarning("Falha na validação do modelo veicular {Nome}. Total de erros: {Count}", 
                    request.Descricao, domainNotificationContext.GetNotificationCount());
                return BaseResponse<long>.Erro("Dados inválidos para criação do modelo veicular", new List<string> { "Falha na validação dos dados" });
            }

            await _modeloVeicularRepository.AdicionarAsync(modeloVeicular);

            _logger.LogInformation("Modelo veicular {Nome} criado com sucesso. ID: {Id}", 
                request.Descricao, modeloVeicular.Id);

            return BaseResponse<long>.Ok(modeloVeicular.Id, "Modelo veicular criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar modelo veicular {Nome}", request.Descricao);
            return BaseResponse<long>.Erro("Erro ao criar modelo veicular", new List<string> { ex.Message });
        }
    }
} 
