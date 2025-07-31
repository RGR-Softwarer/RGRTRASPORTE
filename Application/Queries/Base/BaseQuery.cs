using MediatR;

namespace Application.Queries.Base;

public abstract class BaseQuery<TResponse> : IRequest<TResponse>
{
    // Classe simplificada - propriedades de auditoria podem ser adicionadas via behaviors/interceptors
} 
