using MediatR;

namespace Application.Commands.Base;

public abstract class BaseCommand<TResponse> : IRequest<TResponse>
{
    // Classe simplificada - propriedades de auditoria podem ser adicionadas via behaviors/interceptors
}
