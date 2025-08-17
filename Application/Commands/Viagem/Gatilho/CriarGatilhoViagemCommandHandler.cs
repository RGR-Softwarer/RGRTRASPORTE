using MediatR;
using Application.Common;
using Dominio.Interfaces.Infra.Data;
using GatilhoViagemEntity = Dominio.Entidades.Viagens.Gatilho.GatilhoViagem;
using Microsoft.Extensions.Logging;
using Dominio.Interfaces;

namespace Application.Commands.Viagem.Gatilho;

public class CriarGatilhoViagemCommandHandler : IRequestHandler<CriarGatilhoViagemCommand, BaseResponse<long>>
{
    private readonly IGenericRepository<GatilhoViagemEntity> _repository;
    private readonly ILogger<CriarGatilhoViagemCommandHandler> _logger;
    private readonly INotificationContext _notificationContext;

    public CriarGatilhoViagemCommandHandler(
        IGenericRepository<GatilhoViagemEntity> repository,
        ILogger<CriarGatilhoViagemCommandHandler> logger,
        INotificationContext notificationContext)
    {
        _repository = repository;
        _logger = logger;
        _notificationContext = notificationContext;
    }

    public async Task<BaseResponse<long>> Handle(CriarGatilhoViagemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando criação do gatilho de viagem {Descricao}", request.Descricao);

            // Usar Factory Method com validação
            var (gatilho, sucesso) = GatilhoViagemEntity.CriarGatilhoViagemComValidacao(
                request.Descricao,
                request.VeiculoId,
                request.MotoristaId,
                request.LocalidadeOrigemId,
                request.LocalidadeDestinoId,
                request.HorarioSaida,
                request.HorarioChegada,
                request.ValorPassagem,
                request.QuantidadeVagas,
                (decimal)request.Distancia,
                request.DescricaoViagem,
                request.PolilinhaRota,
                request.DiasSemana,
                _notificationContext);

            if (!sucesso || gatilho == null)
            {
                _logger.LogWarning("Falha na validação do gatilho de viagem {Descricao}. Total de erros: {Count}", 
                    request.Descricao, _notificationContext.GetNotificationCount());
                return BaseResponse<long>.Erro("Dados inválidos para criação do gatilho de viagem", new List<string> { "Falha na validação dos dados" });
            }

            await _repository.AdicionarAsync(gatilho);

            _logger.LogInformation("Gatilho de viagem {Descricao} criado com sucesso. ID: {Id}", 
                request.Descricao, gatilho.Id);

            return BaseResponse<long>.Ok(gatilho.Id, "Gatilho de viagem criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar gatilho de viagem {Descricao}", request.Descricao);
            return BaseResponse<long>.Erro("Erro ao criar gatilho de viagem", new List<string> { ex.Message });
        }
    }
} 
