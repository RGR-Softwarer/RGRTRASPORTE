using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Viagem;

public class ViagemCriadaEventHandler : INotificationHandler<ViagemCriadaEvent>
{
    private readonly ILogger<ViagemCriadaEventHandler> _logger;

    public ViagemCriadaEventHandler(ILogger<ViagemCriadaEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ViagemCriadaEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Viagem criada: ID {ViagemId}, Data {DataViagem}, Origem {Origem}, Destino {Destino}, " +
            "Veículo {VeiculoId}, Motorista {MotoristaId}, Valor {ValorViagem}, " +
            "Quantidade de Passageiros {QuantidadePassageiros}",
            notification.ViagemId,
            notification.DataViagem,
            notification.Origem,
            notification.Destino,
            notification.VeiculoId,
            notification.MotoristaId,
            notification.ValorViagem,
            notification.PassageirosIds.Count);

        // Aqui você pode adicionar lógica adicional para processar o evento
        // Por exemplo: enviar notificações, atualizar cache, etc.

        return Task.CompletedTask;
    }
}
